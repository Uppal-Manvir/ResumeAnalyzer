import undetected_chromedriver as uc
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.chrome.service import Service  # Helps launch ChromeDriver
from webdriver_manager.chrome import ChromeDriverManager  # Downloads and manages ChromeDriver automatically
from selenium.webdriver.common.by import By  # Used to locate HTML elements
import time  
from db.postgres import insert_job

def scrape_indeed_jobs():

    # Setup Chrome options
    #options = webdriver.ChromeOptions()
    #options.add_argument('--headless')  
    options = uc.ChromeOptions()
    options.add_argument('--disable-popup-blocking')

    # Start driver
    driver = uc.Chrome(
        options=options
    )


    url = "https://ca.indeed.com/jobs?q=developer&l=Brampton%2C+ON&radius=100&sort=date"

    # HEADERS = {
    #     "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/115.0",
    #     "Accept": "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8",
    #     "Accept-Language": "en-US,en;q=0.5",
    #     "Connection": "keep-alive",
    #     "Upgrade-Insecure-Requests": "1",
    # }

    driver.get(url)

    print("Waiting pls work.")
    
    time.sleep(15)  # Let page load a bit

    page = 1
    
    while(True): #loop through all jobs on all page: TODO: update to only see new jobs

        job_cards = driver.find_elements(By.CLASS_NAME, "job_seen_beacon")

        print(f"Current page: {page}")
        print(f"Found {len(job_cards)} job postings on the page.")
        
        print("Acc worked tf?")

        for job_card in job_cards:
            try:
                a_tag = job_card.find_element(By.TAG_NAME, "a")  # Find <a> tag
                href = a_tag.get_attribute("href")  # Get the href link
                span = a_tag.find_element(By.TAG_NAME, "span")  # Find <span> inside <a> for title
                title = span.text.strip()
                job_description = extract_job_desc_indeed(driver, href)
                
            except Exception as e:
                print(f"Error extracting job info: {e}")
                continue

            print(f"Job Title: {title}")
            print(f"Job Link: {href}")
            print(f"Job Desc: {job_description[:400]}")
            print("------------")
            insert_job({
                "title": title,
                #"company": job_company,
                #"location": job_location,
                "url": href,
                "description": job_description
            })
            score = score_job(job_desc)
        try:
            next_button = driver.find_element(By.CSS_SELECTOR, "a[data-testid='pagination-page-next']")
            driver.execute_script("arguments[0].click();", next_button)
            time.sleep(5)
        except Exception as e:

            print("No more pages or error finding Next button.  " + next_label)
            break
    while True:
        print("DONEEE")
        time.sleep(20)


def extract_job_desc_indeed(driver, job_url):
    try:

        driver.execute_script("window.open('');") 
        driver.switch_to.window(driver.window_handles[1]) 
        driver.get(job_url)
        time.sleep(2)
        WebDriverWait(driver, 10).until(
            EC.presence_of_element_located((By.ID, "jobDescriptionText"))
        )
        desc_elem = driver.find_element(By.ID, "jobDescriptionText")
        job_description = desc_elem.text.strip()

        driver.close()
        driver.switch_to.window(driver.window_handles[0])

        return job_description

    except Exception as e:
        print(f"failed at extracting desc from {job_url}: {e}")
        try:
            driver.close()
            driver.switch_to.window(driver.window_handles[0])
        except:
            pass
        return none


def scrape_indeed_jobsTest():



    for x in range(2): #loop through all jobs on all page: TODO: update to only see new jobs

        try:

            href = "https://ca.indeed.com/rc/clk?jk=ecea07b2879df218&bb=pRVzlPTmbIQS4wiuFyOl15cRN0Z2wO5dLqFSEvco_PL5zVeBFHWAZwpu0GbsKIVW6Nv9NbGRXPGjCz0x-ibzsRCgfI0dNsobsA1fRrYojLFX8pFjKa6mtSkglzkmhn-L&xkcb=SoBa67M3yPU8a-y4Y50LbzkdCdPP&fccid=aa8e1746ddb73d2f&cmp=Bytecraft-Soultions&ti=Software+Engineer&vjs=3"
            title = "RQ09096 - Software Developer - ETL - Senior"
            job_description = "Closing Date/Time: 2025-05-06, 10:00 a.m. EST Hybrid: 3 Days onsite / 2 days remote\ Must Have Skills: Knowledge of PeopleSoft HCM operating systems, delivered application functionality , configurations and packaged application integration, focus on modules Workforce Administration and Position management as well as Benefit and payroll In-depth knowledge and experience of PeopleSoft system develop"
        except Exception as e:
            print(f"Error extracting job info: {e}")
            continue

        print(f"Job Title: {title}")
        print(f"Job Link: {href}")
        print(f"Job Desc: {job_description[:400]}")
        print("------------")
        insert_job({
            "title": title,
            "url": href,
            "description": job_description
        })
        print("DONEEEE ----------------------------")


