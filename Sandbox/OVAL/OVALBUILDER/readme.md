The tool OVALBuilder:
- Takes a CVE-ID (or MS-ID, oldschool before Security Guidance) as input
- Uses a specified target filename (i.e.: win32k.sys, preferred method) as input (otherwise, it does best effort to guess* it (so lot of hardcoding))

Then it will visit Microsoft website (security-guidance, or old security/bulletin)
For old bulletins, it will scrap the webpages to retrieve the affected product names (and/or use the list of CPEs obtained from NVD). Now this list can be obtained with the MS API.

It will scrap to list the KB numbers. (now can be obtained via MS API)

While scraping, it will find the updated files and versions (if available) directly from the tables on Microsoft website, or download/parse the CSVs.
Otherwise, it will visit the catalog, download the patches, extract them (with expand or 7zip), and search in the extracted cab/msp/msi/msu.

It will use XORCISM (an SQL version of the CIS OVAL Repo) to retrieve the existing corresponding OVAL Definitions, Tests, Objects, States IDs, or creates new ones.

Finally, it will write the OVAL Definition XML file.