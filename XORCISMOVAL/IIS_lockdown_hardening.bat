@ECHO OFF
echo IIS lockdown hardening script example
echo *************************************************************************************
echo Copyright (C) 2013 Jerome Athias, McAfee Foundstone
pause Review and edit this script before using it
echo See http://www.iis.net/configreference
echo Use Best Practices to configure TLS (SSL) and ciphers
echo Define Whitelists of files extensions, MIME types, URIs...
echo *************************************************************************************
echo Backup of IIS settings in IISBackup.bak
%windir%\system32\inetsrv\appcmd.exe add backup IISBackup.bak
echo Keep IIS websites XML configuration data in IISwebsites_config.xml
%windir%\system32\inetsrv\appcmd.exe list site /config /xml > IISwebsites_config.xml
cd %windir%\system32\inetsrv
appcmd.exe set config /commit:WEBROOT /section:trust /level:Medium
appcmd configure trace /disable
appcmd.exe set config -section:system.applicationHost/sites /siteDefaults.traceFailedRequestsLogging.enabled:"False" /commit:apphost
appcmd.exe set config /section:system.webServer/handlers /accessPolicy:Read
echo Note that Read,Script should be required
appcmd.exe set config /section:directoryBrowse /enabled:false
appcmd.exe set config /commit:WEBROOT /section:system.web/authentication /mode:Forms
appcmd.exe set config /commit:WEBROOT /section:system.web/authentication /forms.requireSSL:True
appcmd.exe set config /commit:WEBROOT /section:system.web/authentication /forms.cookieless:UseCookies
appcmd.exe set config /commit:WEBROOT /section:sessionState /cookieless:UseCookies
appcmd.exe set config /section:asp /keepSessionIdSecure:True
appcmd.exe set config /commit:WEBROOT /section:system.web/authentication /forms.protection:All
appcmd.exe set config /commit:WEBROOT /section:system.web/authentication /forms.slidingExpiration:True
appcmd.exe set config /commit:WEBROOT /section:machineKey /validation:3DES
appcmd.exe set config /commitPath:APPHOST /section:access /sslFlags:Ssl
appcmd.exe set config /section:requestfiltering /requestlimits.maxallowedcontentlength:30000000
appcmd.exe set config /section:requestfiltering /requestlimits.maxquerystring:2048
appcmd.exe set config /section:requestfiltering /requestlimits.maxurl:4096
appcmd.exe set config /section:requestfiltering /fileExtensions.allowunlisted:false
appcmd.exe set config /section:requestfiltering /fileExtensions.applyToWebDAV:true
appcmd.exe set config /section:requestfiltering /allowdoubleescaping:false
appcmd.exe set config /section:requestfiltering /allowhighbitcharacters:false
echo See http://www.iis.net/configreference/system.webserver/security/requestfiltering/denyurlsequences
appcmd.exe set config /section:system.webServer/security/requestFiltering /+"denyUrlSequences.[sequence='..']" 
appcmd.exe set config /section:system.webServer/security/requestFiltering /+"denyUrlSequences.[sequence=':']" 
appcmd.exe set config /section:system.webServer/security/requestFiltering /+"denyUrlSequences.[sequence='\']"
echo To allow a file extension use:
echo appcmd.exe set config /section:requestfiltering /+fileExtensions.[fileextension=' .xxx ',allowed='true'] 
echo appcmd.exe set config "Default Web Site" -section:system.webServer/staticContent /+"[fileExtension='xxx',mimeType='text/plain']"
echo To disallow a file extension use:
echo appcmd.exe set config /section:requestfiltering /-fileExtensions.[fileextension=' .xxx '] 
appcmd.exe set config /section:system.applicationHost/applicationPools /applicationPoolDefaults.processModel.identityType:ApplicationPoolIdentity
appcmd.exe set config /section:system.webServer/httpProtocol /+customHeaders.[name='X-Content-Type-Options',value='nosniff'] /commit:apphost
appcmd.exe set config /section:system.webServer/security/requestFiltering /+"requestLimits.headerLimits.[header='Content-type',sizeLimit='100']"
appcmd.exe set config /commit:MACHINE -section:system.web/deployment /retail:true
appcmd.exe set config -section:system.web/compilation /!debug:True
appcmd.exe set config -section:system.web/customErrors /mode:"On"
appcmd.exe set config "Default Web Site" -section:system.webServer/httpProtocol /+"customHeaders.[name='X-Frame-Options',value='SAMEORIGIN']"
appcmd.exe set config "Default Web Site" -section:system.webServer/staticContent /clientCache.cacheControlMode:"DisableCache"
appcmd.exe set config /section:isapiCgiRestriction /notListedCGIsAllowed:False /notlistedISAPIsAllowed:False
appcmd.exe set config /section:requestfiltering /+verbs.[verb='TRACE',allowed='false']
appcmd.exe set config /section:requestfiltering /+verbs.[verb='TRACK',allowed='false']
appcmd.exe set config /section:requestfiltering /+verbs.[verb='LOCK',allowed='false']
appcmd.exe set config /section:requestfiltering /+verbs.[verb='UNLOCK',allowed='false']
appcmd.exe set config /section:requestfiltering /+verbs.[verb='PROPFIND',allowed='false']
appcmd.exe set config /section:requestfiltering /+verbs.[verb='SEARCH',allowed='false']
appcmd.exe set config /section:requestfiltering /+verbs.[verb='PUT',allowed='false']
appcmd.exe set config /section:requestfiltering /+verbs.[verb='DELETE',allowed='false']
appcmd.exe set config /section:requestfiltering /+verbs.[verb='HEAD',allowed='true']
appcmd.exe set config /section:requestfiltering /+verbs.[verb='OPTIONS',allowed='false']
appcmd.exe set config /section:requestfiltering /+verbs.[verb='PROPPATCH',allowed='false']
appcmd.exe set config /section:requestfiltering /+verbs.[verb='MKCOL',allowed='false']
appcmd.exe set config /section:requestfiltering /+verbs.[verb='COPY',allowed='false']
appcmd.exe set config /section:requestfiltering /+verbs.[verb='MOVE',allowed='false'] 
appcmd.exe set config /section:requestfiltering /+verbs.[verb='GET',allowed='true']
appcmd.exe set config /section:requestfiltering /+verbs.[verb='POST',allowed='true'] 
appcmd.exe set config /section:httpLogging /dontLog:False /selectiveLogging:LogAll
appcmd.exe set config -section:system.webServer/advancedLogging/server /enabled:"True" /commit:apphost
echo *************************************************************************************
echo To restore the backup use the command: appcmd.exe restore backup IISBackup.bak
echo To rollback use the command: appcmd.exe add sites /in < IISwebsites_config.xml
echo *************************************************************************************
iisreset