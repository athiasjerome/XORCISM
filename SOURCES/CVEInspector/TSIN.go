package main
//Tiny Security Inspector for NIST NVDCVE - v0.1
// Use latest CVEs information and compare them to your predefined list of softwares/technologies
// Jerome Athias, 2020

import (
	"bufio"
	"log"
	"encoding/json"
	"fmt"
	"os"
	"io/ioutil"
	"strings"
	"net/http"
	"io"
	"archive/zip"
	"path/filepath"
	"time"
)

// NvdJSON is a struct of NVD JSON
// https://csrc.nist.gov/schema/nvd/feed/1.1/nvd_cve_feed_json_1.1.schema
// Based on https://github.com/kotakanbe/go-cve-dictionary
type NvdJSON struct {
	CveDataType         string    `json:"CVE_data_type"`
	CveDataFormat       string    `json:"CVE_data_format"`
	CveDataVersion      string    `json:"CVE_data_version"`
	CveDataNumberOfCVEs string    `json:"CVE_data_numberOfCVEs"`
	CveDataTimestamp    string    `json:"CVE_data_timestamp"`
	CveItems            []CveItem `json:"CVE_Items"`
}

// CveItem is a struct of NvdJSON>CveItems
type CveItem struct {
	Cve struct {
		DataType    string `json:"data_type"`
		DataFormat  string `json:"data_format"`
		DataVersion string `json:"data_version"`
		CveDataMeta struct {
			ID       string `json:"ID"`
			ASSIGNER string `json:"ASSIGNER"`
		} `json:"CVE_data_meta"`
		Affects struct {
			Vendor struct {
				VendorData []struct {
					VendorName string `json:"vendor_name"`
					Product    struct {
						ProductData []struct {
							ProductName string `json:"product_name"`
							Version     struct {
								VersionData []struct {
									VersionValue string `json:"version_value"`
								} `json:"version_data"`
							} `json:"version"`
						} `json:"product_data"`
					} `json:"product"`
				} `json:"vendor_data"`
			} `json:"vendor"`
		} `json:"affects"`
		Problemtype struct {
			ProblemtypeData []struct {
				Description []struct {
					Lang  string `json:"lang"`
					Value string `json:"value"`
				} `json:"description"`
			} `json:"problemtype_data"`
		} `json:"problemtype"`
		References struct {
			ReferenceData []struct {
				URL  string   `json:"url"`
				TAGS []string `json:"tags"`
			} `json:"reference_data"`
		} `json:"references"`
		Description struct {
			DescriptionData []struct {
				Lang  string `json:"lang"`
				Value string `json:"value"`
			} `json:"description_data"`
		} `json:"description"`
	} `json:"cve"`
	Configurations struct {
		CveDataVersion string `json:"CVE_data_version"`
		Nodes          []struct {
			Operator string `json:"operator"`
			Negate   bool   `json:"negate"`
			Cpes     []struct {
				Vulnerable            bool   `json:"vulnerable"`
				Cpe23URI              string `json:"cpe23Uri"`
				VersionStartExcluding string `json:"versionStartExcluding"`
				VersionStartIncluding string `json:"versionStartIncluding"`
				VersionEndExcluding   string `json:"versionEndExcluding"`
				VersionEndIncluding   string `json:"versionEndIncluding"`
			} `json:"cpe_match"`
			Children []struct {
				Operator string `json:"operator"`
				Cpes     []struct {
					Vulnerable            bool   `json:"vulnerable"`
					Cpe23URI              string `json:"cpe23Uri"`
					VersionStartExcluding string `json:"versionStartExcluding"`
					VersionStartIncluding string `json:"versionStartIncluding"`
					VersionEndExcluding   string `json:"versionEndExcluding"`
					VersionEndIncluding   string `json:"versionEndIncluding"`
				} `json:"cpe_match"`
			} `json:"children,omitempty"`
		} `json:"nodes"`
	} `json:"configurations"`
	Impact struct {
		BaseMetricV3 struct {
			CvssV3 struct {
				Version               string  `json:"version"`
				VectorString          string  `json:"vectorString"`
				AttackVector          string  `json:"attackVector"`
				AttackComplexity      string  `json:"attackComplexity"`
				PrivilegesRequired    string  `json:"privilegesRequired"`
				UserInteraction       string  `json:"userInteraction"`
				Scope                 string  `json:"scope"`
				ConfidentialityImpact string  `json:"confidentialityImpact"`
				IntegrityImpact       string  `json:"integrityImpact"`
				AvailabilityImpact    string  `json:"availabilityImpact"`
				BaseScore             float64 `json:"baseScore"`
				BaseSeverity          string  `json:"baseSeverity"`
			} `json:"cvssV3"`
			ExploitabilityScore float64 `json:"exploitabilityScore"`
			ImpactScore         float64 `json:"impactScore"`
		} `json:"baseMetricV3"`
		BaseMetricV2 struct {
			CvssV2 struct {
				Version               string  `json:"version"`
				VectorString          string  `json:"vectorString"`
				AccessVector          string  `json:"accessVector"`
				AccessComplexity      string  `json:"accessComplexity"`
				Authentication        string  `json:"authentication"`
				ConfidentialityImpact string  `json:"confidentialityImpact"`
				IntegrityImpact       string  `json:"integrityImpact"`
				AvailabilityImpact    string  `json:"availabilityImpact"`
				BaseScore             float64 `json:"baseScore"`
			} `json:"cvssV2"`
			Severity                string  `json:"severity"`
			ExploitabilityScore     float64 `json:"exploitabilityScore"`
			ImpactScore             float64 `json:"impactScore"`
			ObtainAllPrivilege      bool    `json:"obtainAllPrivilege"`
			ObtainUserPrivilege     bool    `json:"obtainUserPrivilege"`
			ObtainOtherPrivilege    bool    `json:"obtainOtherPrivilege"`
			UserInteractionRequired bool    `json:"userInteractionRequired"`
		} `json:"baseMetricV2"`
	} `json:"impact"`
	PublishedDate    string `json:"publishedDate"`
	LastModifiedDate string `json:"lastModifiedDate"`
}


type Definitions struct {
    Definitions []item `json:"CVE_Items"`
}

type item struct {
	cve cve `json:"cve"`
}

type cve struct {
	//CVE_data_meta   string `json:"CVE_data_meta"`
	CVE_data_meta CVE_data_meta `json:"CVE_data_meta"`
	publishedDate string `json:"publishedDate"`
	lastModifiedDate string `json:"lastModifiedDate"`
}

type CVE_data_meta struct {
	ID string `json:"ID"`
	ASSIGNER string `json:"ASSIGNER"`
  }


/*
// Cwe has CweID
type Cwe struct {
	//gorm.Model `json:"-" xml:"-"`
	NvdXMLID   uint `json:"-" xml:"-"`
	NvdJSONID  uint `json:"-" xml:"-"`
	JvnID      uint `json:"-" xml:"-"`

	CweID string
}

// Cpe is Child model of Jvn/Nvd.
// see https://www.ipa.go.jp/security/vuln/CPE.html
// In NVD JSON,
// configurations>nodes>cpe>vulnerable: true
type Cpe struct {
	gorm.Model `json:"-" xml:"-"`
	JvnID      uint `json:"-" xml:"-"`
	NvdXMLID   uint `json:"-" xml:"-"`
	NvdJSONID  uint `json:"-" xml:"-"`

	CpeBase
	EnvCpes []EnvCpe
}
*/

//********************************************************************************************************************
func main() {
	fmt.Printf("NVDCVE Offline Vulnerability Scanner v0.1 - Jerome Athias, 2020\n")

	var downloadneeded bool
	fileUrl := "https://nvd.nist.gov/feeds/json/cve/1.1/nvdcve-1.1-recent.json.zip"	//Hardcoded
	localFilename := "nvdcve-1.1-recent.json"	//Hardcoded
	now := time.Now()
	var cutoff = 8 * time.Hour	//Hardcoded
	
	fi, err := os.Stat(localFilename+".zip");
	if err != nil {
		downloadneeded = true
		fmt.Println(err)
	} else {
		modifiedtime := fi.ModTime()
		fmt.Println("Local File "+localFilename+" Last modified: ", modifiedtime)

		if diff := now.Sub(fi.ModTime()); diff > cutoff {

			// get the size of the local file
			size := fi.Size()
			//fmt.Printf("Local File Size:%v\n",size)

			// Send an HEAD request to obtain remote file size (do not overload the server)
			res, err := http.Head(fileUrl)
			if err != nil {
				panic(err)
			}
			contentlength:=res.ContentLength
			//fmt.Printf("ContentLength:%v\n",contentlength)

			if(contentlength!=size) {
				downloadneeded = true
			}
		}
	}

	if(downloadneeded) {
		if err := DownloadFile(localFilename+".zip", fileUrl); err != nil {
			panic(err)
		}
		files, err := Unzip(localFilename+".zip", "nvdcve/")
		if err != nil {
			log.Fatal(err)
		}
		fmt.Println("Unzipped:\n" + strings.Join(files, "\n"))
	}
    

	var keywords []string
	// Read the input file with technologies/softwares names of our applications
	file, err := os.Open("input_technologies.txt")	//Hardcoded
    if err != nil {
        log.Fatal(err)
    }
    defer file.Close()

    scanner := bufio.NewScanner(file)
    for scanner.Scan() {
		//fmt.Println(scanner.Text())
		keywords = append(keywords, scanner.Text())
    }

    if err := scanner.Err(); err != nil {
        log.Fatal(err)
    }

	// Open our jsonFile
	jsonFile, err := os.Open("nvdcve/"+localFilename)	//Hardcoded

	// if we os.Open returns an error then handle it
	if err != nil {
		fmt.Println(err)
	}
	//fmt.Println("Successfully Opened "+localFilename)
	// defer the closing of our jsonFile so that we can parse it later on
	defer jsonFile.Close()

	// read our opened jsonFile as a byte array.
	byteValue, _ := ioutil.ReadAll(jsonFile)

	// we initialize our Definitions array
	var nvd NvdJSON

	// we unmarshal our byteArray which contains our
	// jsonFile's content into 'nvd' which we defined above
	json.Unmarshal(byteValue, &nvd)

	// we iterate through every definition within our nvd array
	for i := 0; i < len(nvd.CveItems); i++ {
		var cveDescription string = nvd.CveItems[i].Cve.Description.DescriptionData[0].Value

		for _, keyword := range keywords {
			if strings.Contains(cveDescription, keyword) {
				//TODO: check CPEs, Affects
				//Filter out (e.g.: "plugin")

				//TODO: put your alerts/actions here (e.g.: query your CMDB, send email/Slack msg, create Jira ticket, scan SWID tags...)
				fmt.Println("CVEID: " + nvd.CveItems[i].Cve.CveDataMeta.ID)
				//fmt.Println("CVE DatePublished: " + nvd.CveItems[i].PublishedDate)
				fmt.Println("KeywordFound: " + keyword)
				fmt.Println("CVE Description: " + nvd.CveItems[i].Cve.Description.DescriptionData[0].Value)
				fmt.Println("==========================================================================================================================================================")
			}
		}
		
	}
}

// DownloadFile will download a url to a local file. It's efficient because it will
// write as it downloads and not load the whole file into memory.
func DownloadFile(filepath string, url string) error {
	//For GZIP see https://github.com/kotakanbe/go-cve-dictionary/blob/master/fetcher/fetcher.go
	//https://golang.org/src/net/http/transport.go#L173

    // Get the data
    resp, err := http.Get(url)
    if err != nil {
        return err
    }
    defer resp.Body.Close()

    // Create the file
    out, err := os.Create(filepath)
    if err != nil {
        return err
    }
    defer out.Close()

    // Write the body to file
    _, err = io.Copy(out, resp.Body)
    return err
}

// Unzip will decompress a zip archive, moving all files and folders
// within the zip file (parameter 1) to an output directory (parameter 2).
func Unzip(src string, dest string) ([]string, error) {

    var filenames []string

    r, err := zip.OpenReader(src)
    if err != nil {
        return filenames, err
    }
    defer r.Close()

    for _, f := range r.File {

        // Store filename/path for returning and using later on
        fpath := filepath.Join(dest, f.Name)

        // Check for ZipSlip. More Info: http://bit.ly/2MsjAWE
        if !strings.HasPrefix(fpath, filepath.Clean(dest)+string(os.PathSeparator)) {
            return filenames, fmt.Errorf("%s: illegal file path", fpath)
        }

        filenames = append(filenames, fpath)

        if f.FileInfo().IsDir() {
            // Make Folder
            os.MkdirAll(fpath, os.ModePerm)
            continue
        }

        // Make File
        if err = os.MkdirAll(filepath.Dir(fpath), os.ModePerm); err != nil {
            return filenames, err
        }

        outFile, err := os.OpenFile(fpath, os.O_WRONLY|os.O_CREATE|os.O_TRUNC, f.Mode())
        if err != nil {
            return filenames, err
        }

        rc, err := f.Open()
        if err != nil {
            return filenames, err
        }

        _, err = io.Copy(outFile, rc)

        // Close the file without defer to close before next iteration of loop
        outFile.Close()
        rc.Close()

        if err != nil {
            return filenames, err
        }
    }
    return filenames, nil
}

