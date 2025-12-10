# n8n CRM Exception RAG Workflow - Setup Guide

This guide helps you configure the backend requirements for the **CRM Exception RAG Workflow** in n8n.

## 0. Import the Workflow
1.  In your n8n dashboard, click **Add Workflow** > **Import from File**.
2.  Select the file: `CRM_ExceptionFlow\CRM_ExceptionFlow\Workflows\CRM_Exception_RAG_Workflow.json`.

## 1. Database Setup
The workflow relies on a SQL Server Stored Procedure to find similar historical exceptions.

1.  **Locate the SQL Script**:
    -   Go to `CRM_ExceptionFlow\CRM_ExceptionFlow\SQL\sp_SearchSimilarExceptions.sql`.
2.  **Execute in SQL Server**:
    -   Open SQL Server Management Studio (SSMS).
    -   Connect to your `CRM Database`.
    -   Open the `.sql` file and click **Execute** (F5).
    -   Verify it reports "Command(s) completed successfully."

## 2. n8n Configuration
The workflow requires two main credentials:

### A. SQL Server Credential
1.  In n8n, go to **Credentials**.
2.  Create a new **Microsoft SQL** credential.
3.  Name it: `CRM Database` (matches the workflow node).
4.  Enter your database connection details (Host, Database, User, Password).

### B. OpenAI API Key
1.  Open the **Call OpenAI API** node in the workflow.
2.  In the **Headers** section, look for `Authorization`.
3.  Replace the placeholder value with your actual API Key:
    `Bearer sk-YOUR-OPENAI-API-KEY`

## 3. Testing the Workflow
1.  **Activate** the workflow in n8n.
2.  Send a test POST request to the webhook URL (e.g., using Postman):
    ```json
    POST https://start.n8n.cloud/webhook/exception-recommendation
    Content-Type: application/json

    {
      "exceptionId": 123,
      "projectId": "PROJ-001",
      "module": "Sales",
      "description": "System timeout when saving deal"
    }
    ```
3.  Check the **Executions** tab in n8n for successful runs.
