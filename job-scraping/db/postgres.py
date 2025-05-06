
import psycopg2
from psycopg2.extras import RealDictCursor
import os
from dotenv import load_dotenv

load_dotenv()

DB_HOST = os.getenv("DB_HOST", "localhost")
DB_NAME = os.getenv("DB_NAME", "your_db_name")
DB_USER = os.getenv("DB_USER", "your_user")
DB_PASSWORD = os.getenv("DB_PASSWORD", "your_password")
DB_PORT = os.getenv("DB_PORT", "5432")

def get_connection():
    return psycopg2.connect(
        host=DB_HOST,
        dbname=DB_NAME,
        user=DB_USER,
        password=DB_PASSWORD,
        port=DB_PORT,
        cursor_factory=RealDictCursor
    )

def insert_job(job):
    conn = get_connection()
    cur = conn.cursor()
    try:
        cur.execute("""
            INSERT INTO jobs (job_title, job_url, job_desc)
            VALUES (%s, %s, %s)
            ON CONFLICT (job_url) DO NOTHING
        """, (
            job['title'],
            job['url'],
            job['description']
        ))
        conn.commit()
    except Exception as e:
        print(f"Error inserting job: {e}")
    finally:
        cur.close()
        conn.close()
