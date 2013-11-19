--http://www.postgresonline.com/journal/archives/219-SQL-Server-to-PostgreSQL-Converting-table-structure.html
CREATE OR REPLACE FUNCTION convert_mssqlddl2pgsql(sql text, change_camel_under boolean, default_not_nulls boolean DEFAULT true, replace_dbo_with_schema text DEFAULT NULL)
  RETURNS text AS
$$
DECLARE 
    var_sql text := sql;
    r record;
BEGIN
    IF change_camel_under THEN
        -- only match capitals [A-Z] that are preceded and followed by lower case [a-z]
        -- replace the whole match with preceding lowercase _uppercase following lowercase
        var_sql := regexp_replace(var_sql, E'([a-z])([A-Z])([a-z]?)', E'\\1\_\\2\\3','g'); 
    END IF;
    var_sql := lower(var_sql);
    var_sql := replace(var_sql,'[dbo].', COALESCE('[' || replace_dbo_with_schema || '].',''));
    var_sql := replace(var_sql,'on [primary]', '');
	var_sql := replace(var_sql,'[nvarchar](max)', 'text');
	var_sql := replace(var_sql,'[datetimeoffset](7)', '[datetimeoffset]');
    FOR r IN (SELECT * FROM ( VALUES ('datetimeoffset', 'timestamp with time zone', 'CURRENT_TIMESTAMP'), 
			('datetime', 'timestamp with time zone', 'CURRENT_TIMESTAMP'),    
            ('bit', 'boolean', 'true'),
            ('varchar(max)', 'text', ''), 
            ('nvarchar', 'varchar', ''), 
            ('tinyint','smallint', '0') ,
            ('[int] identity(1,1)', 'serial', NULL)
            ) As f(ms, pg, def)) LOOP
        IF default_not_nulls AND r.def IS NOT NULL THEN
            var_sql := replace(var_sql, '[' || r.ms || '] not null', '[' || r.ms || '] not null DEFAULT ' || r.def);  
        END IF;
        var_sql := replace(var_sql, '[' || r.ms || ']',r.pg) ;
        var_sql := replace(var_sql, r.ms ,r.pg) ;
    END LOOP;
    var_sql := regexp_replace(var_sql, '[\[\]]','','g');
    var_sql := regexp_replace(var_sql,'(primary key|unique) (clustered|nonclustered)', E'\\1', 'g');
    --get rid of all that WITH (PAD_INDEX ...) that sql server generates for tables
    -- so basically match any phrase WITH ("everything not containing )" ) 
    var_sql := regexp_replace(var_sql, 'with \([^\)]+\)', '','g');
    -- get rid of asc in column constraints
    var_sql := regexp_replace(var_sql, '([a-z]+) asc', E'\\1','g');
    
    -- get rid of collation
    -- for PostgreSQL 9.1 might want
    -- to just change it to 9.1 syntax
    var_sql := regexp_replace(var_sql, 'collate [a-z0-9\_]+', '','g');
    RETURN var_sql;
    
END;
$$
  LANGUAGE plpgsql IMMUTABLE;