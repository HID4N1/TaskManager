#!/usr/bin/env python3
"""
Script to take screenshots of all pages in the TaskManager application
"""
import asyncio
import sys
import os
from playwright.async_api import async_playwright
import time

# Base URL - try multiple ports
POSSIBLE_PORTS = [8080, 5000, 5001, 7000, 7001, 5002, 5003]
BASE_URL = "http://localhost:8080"  # Default to 8080
HTTPS_URL = None

# Test credentials
ADMIN_EMAIL = "admin@taskmanager.com"
ADMIN_PASSWORD = "Admin123!"

# Screenshot directory
SCREENSHOT_DIR = "screenshots"

async def take_screenshot(page, url, filename, wait_time=2):
    """Take a screenshot of a page"""
    try:
        print(f"Taking screenshot: {filename}")
        await page.goto(url, wait_until="networkidle", timeout=30000)
        await page.wait_for_timeout(wait_time * 1000)  # Wait for any animations
        await page.screenshot(path=os.path.join(SCREENSHOT_DIR, filename), full_page=True)
        print(f"✓ Screenshot saved: {filename}")
        return True
    except Exception as e:
        print(f"✗ Error taking screenshot {filename}: {e}")
        return False

async def login(page, base_url):
    """Login as admin"""
    try:
        print("Logging in as admin...")
        await page.goto(f"{base_url}/Auth/Login", wait_until="domcontentloaded", timeout=30000)
        await page.wait_for_timeout(3000)  # Wait for page to fully load
        
        # Get all input fields to debug
        inputs = await page.query_selector_all('input')
        print(f"Found {len(inputs)} input fields")
        
        # Fill email - try by type first, then by position
        try:
            email_input = await page.query_selector('input[type="email"], input#Email, input[name="Email"], input.form-control:first-of-type')
            if email_input:
                await email_input.fill(ADMIN_EMAIL)
            else:
                # Try first text input
                inputs = await page.query_selector_all('input.form-control')
                if len(inputs) > 0:
                    await inputs[0].fill(ADMIN_EMAIL)
        except Exception as e:
            print(f"Error filling email: {e}")
            return False
        
        # Fill password
        try:
            password_input = await page.query_selector('input[type="password"], input#Password, input[name="Password"]')
            if password_input:
                await password_input.fill(ADMIN_PASSWORD)
            else:
                # Try password input by type
                password_inputs = await page.query_selector_all('input[type="password"]')
                if len(password_inputs) > 0:
                    await password_inputs[0].fill(ADMIN_PASSWORD)
        except Exception as e:
            print(f"Error filling password: {e}")
            return False
        
        # Click submit
        try:
            submit_button = await page.query_selector('button[type="submit"], button.btn-primary, input[type="submit"]')
            if submit_button:
                await submit_button.click()
            else:
                # Try by text
                await page.click('button:has-text("Login")')
        except Exception as e:
            print(f"Error clicking submit: {e}")
            return False
        
        # Wait for redirect
        try:
            await page.wait_for_url(f"{base_url}/Project**", timeout=15000)
        except:
            # Check current URL
            current_url = page.url
            print(f"Current URL after login: {current_url}")
            if "/Project" in current_url or "/Home" in current_url:
                pass  # Success
            else:
                return False
        
        print("✓ Login successful")
        return True
    except Exception as e:
        print(f"✗ Login failed: {e}")
        # Take a screenshot of the login page for debugging
        try:
            await page.screenshot(path=os.path.join(SCREENSHOT_DIR, "debug_login.png"), full_page=True)
            print("Debug screenshot saved: debug_login.png")
        except:
            pass
        return False

async def get_project_ids(page, base_url):
    """Get project IDs from the project list"""
    try:
        await page.goto(f"{base_url}/Project", wait_until="networkidle")
        await page.wait_for_timeout(1000)
        
        # Find all project links
        project_links = await page.query_selector_all('a[href*="/Project/Details/"]')
        project_ids = []
        for link in project_links:
            href = await link.get_attribute('href')
            if href:
                # Extract GUID from URL
                import re
                match = re.search(r'/Project/Details/([a-f0-9-]+)', href)
                if match:
                    project_ids.append(match.group(1))
        
        print(f"Found {len(project_ids)} projects")
        return project_ids[:3]  # Return first 3 projects
    except Exception as e:
        print(f"Error getting project IDs: {e}")
        return []

async def get_task_ids(page, base_url):
    """Get task IDs from task list"""
    try:
        await page.goto(f"{base_url}/Task", wait_until="networkidle")
        await page.wait_for_timeout(1000)
        
        task_links = await page.query_selector_all('a[href*="/Task/Details/"]')
        task_ids = []
        for link in task_links:
            href = await link.get_attribute('href')
            if href:
                import re
                match = re.search(r'/Task/Details/([a-f0-9-]+)', href)
                if match:
                    task_ids.append(match.group(1))
        
        print(f"Found {len(task_ids)} tasks")
        return task_ids[:3]  # Return first 3 tasks
    except Exception as e:
        print(f"Error getting task IDs: {e}")
        return []

async def main():
    # Create screenshot directory
    os.makedirs(SCREENSHOT_DIR, exist_ok=True)
    
    # Try to determine which URL works
    base_url = None
    for port in POSSIBLE_PORTS:
        for protocol in ['http', 'https']:
            url = f"{protocol}://localhost:{port}"
            try:
                async with async_playwright() as p:
                    browser = await p.chromium.launch(headless=True, args=['--ignore-certificate-errors'])
                    context = await browser.new_context(ignore_https_errors=True)
                    page = await context.new_page()
                    response = await page.goto(f"{url}/Auth/Login", timeout=5000, wait_until="domcontentloaded")
                    if response and response.status < 500 and response.status != 403:
                        base_url = url
                        await browser.close()
                        print(f"Found app at: {base_url}")
                        break
                    await browser.close()
            except Exception as e:
                continue
        if base_url:
            break
    
    if not base_url:
        print("Could not connect to the application. Please ensure it's running.")
        print(f"Tried: {BASE_URL} and {HTTPS_URL}")
        sys.exit(1)
    
    print(f"Using base URL: {base_url}")
    
    async with async_playwright() as p:
        browser = await p.chromium.launch(headless=True, args=['--ignore-certificate-errors'])
        context = await browser.new_context(
            viewport={'width': 1920, 'height': 1080},
            ignore_https_errors=True
        )
        page = await context.new_page()
        
        # Login
        if not await login(page, base_url):
            print("Failed to login. Exiting.")
            await browser.close()
            sys.exit(1)
        
        screenshots_taken = []
        
        # Auth pages (before login)
        print("\n=== Taking screenshots of Auth pages ===")
        await page.goto(f"{base_url}/Auth/Login")
        await take_screenshot(page, f"{base_url}/Auth/Login", "01_auth_login.png")
        screenshots_taken.append("01_auth_login.png")
        
        await take_screenshot(page, f"{base_url}/Auth/Signup", "02_auth_signup.png")
        screenshots_taken.append("02_auth_signup.png")
        
        # Login again for authenticated pages
        await login(page, base_url)
        
        # Home/Project pages
        print("\n=== Taking screenshots of Project pages ===")
        await take_screenshot(page, f"{base_url}/Project", "03_project_index.png")
        screenshots_taken.append("03_project_index.png")
        
        # Get project IDs and take detail screenshots
        project_ids = await get_project_ids(page, base_url)
        if project_ids:
            for i, project_id in enumerate(project_ids):
                await take_screenshot(page, f"{base_url}/Project/Details/{project_id}", 
                                    f"04_project_details_{i+1}.png")
                screenshots_taken.append(f"04_project_details_{i+1}.png")
        
        # Project Create/Edit pages
        await take_screenshot(page, f"{base_url}/Project/Create", "05_project_create.png")
        screenshots_taken.append("05_project_create.png")
        
        if project_ids:
            await take_screenshot(page, f"{base_url}/Project/Edit/{project_ids[0]}", 
                                "06_project_edit.png")
            screenshots_taken.append("06_project_edit.png")
            
            await take_screenshot(page, f"{base_url}/Project/Delete/{project_ids[0]}", 
                                "07_project_delete.png")
            screenshots_taken.append("07_project_delete.png")
        
        # Task pages
        print("\n=== Taking screenshots of Task pages ===")
        await take_screenshot(page, f"{base_url}/Task", "08_task_index.png")
        screenshots_taken.append("08_task_index.png")
        
        # Kanban view
        if project_ids:
            await take_screenshot(page, f"{base_url}/Task/Kanban/{project_ids[0]}", 
                                "09_task_kanban.png", wait_time=3)
            screenshots_taken.append("09_task_kanban.png")
        
        # Task Create/Edit pages
        await take_screenshot(page, f"{base_url}/Task/Create", "10_task_create.png")
        screenshots_taken.append("10_task_create.png")
        
        task_ids = await get_task_ids(page, base_url)
        if task_ids:
            await take_screenshot(page, f"{base_url}/Task/Details/{task_ids[0]}", 
                                "11_task_details.png")
            screenshots_taken.append("11_task_details.png")
            
            await take_screenshot(page, f"{base_url}/Task/Edit/{task_ids[0]}", 
                                "12_task_edit.png")
            screenshots_taken.append("12_task_edit.png")
            
            await take_screenshot(page, f"{base_url}/Task/Delete/{task_ids[0]}", 
                                "13_task_delete.png")
            screenshots_taken.append("13_task_delete.png")
        
        # Admin pages
        print("\n=== Taking screenshots of Admin pages ===")
        await take_screenshot(page, f"{base_url}/Admin/Users", "14_admin_users.png")
        screenshots_taken.append("14_admin_users.png")
        
        await take_screenshot(page, f"{base_url}/Admin/CreateUser", "15_admin_create_user.png")
        screenshots_taken.append("15_admin_create_user.png")
        
        # Report pages
        print("\n=== Taking screenshots of Report pages ===")
        await take_screenshot(page, f"{base_url}/Report", "16_report_index.png")
        screenshots_taken.append("16_report_index.png")
        
        await take_screenshot(page, f"{base_url}/Report/TaskByStatus", "17_report_task_by_status.png")
        screenshots_taken.append("17_report_task_by_status.png")
        
        if project_ids:
            await take_screenshot(page, f"{base_url}/Report/TaskByStatus?projectId={project_ids[0]}", 
                                "18_report_task_by_status_project.png")
            screenshots_taken.append("18_report_task_by_status_project.png")
        
        await browser.close()
    
    print(f"\n✓ Screenshots completed! {len(screenshots_taken)} screenshots saved in '{SCREENSHOT_DIR}' directory")
    print("\nScreenshots taken:")
    for screenshot in screenshots_taken:
        print(f"  - {screenshot}")

if __name__ == "__main__":
    asyncio.run(main())

