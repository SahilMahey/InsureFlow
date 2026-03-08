# InsureFlow

> A web-based insurance application management system for creating, evaluating, and managing business insurance applications.

---

## Table of Contents

- [Overview](#overview)
- [Roles & Permissions](#roles--permissions)
- [Features](#features)
  - [Applications Tab](#applications-tab)
  - [Rules Tab](#rules-tab)
- [Validation & Error Handling](#validation--error-handling)
- [Setup Instructions](#setup-instructions)
- [Usage](#usage)
- [Notes](#notes)

---

## Overview

InsureFlow allows users to manage business insurance applications end-to-end — from creation and risk evaluation, to premium calculation and policy issuance. It also supports underwriting rule management. The system enforces role-based access control (RBAC) so each user only sees and interacts with functionality relevant to their role.

**Tech Stack:**
- **Frontend:** React, Material-UI (MUI)
- **Backend:** .NET API (runs on port `5002` by default)

---

## Roles & Permissions

| Role | Permissions |
|---|---|
| **Admin** | Full access — manage applications, evaluate, issue policies, and manage rules |
| **Underwriter** | Rules tab only — create, edit, and delete rules; no access to applications |
| **Agent** | Applications tab only — create applications, evaluate, calculate premiums, and issue policies; no access to rules |

---

## Features

### Applications Tab

> Accessible by **Agents** and **Admins**

#### Create Application

Fill out and submit the application form with the following fields:

- Business Name
- Business Type
- Annual Revenue
- Number of Employees
- Years in Business
- Claims History

Click **Create Application** to submit. The `Decision` field and risk score are calculated automatically by the backend.

#### Evaluate Application

Click **Evaluate** to trigger risk scoring and decision calculation via the evaluation API.

#### Calculate Premium

Click **Premium** to fetch the calculated premium. The result is displayed in a popup alert.

#### Issue Policy

Click **Issue** to generate a policy for the application. The assigned policy number is displayed in a popup alert.

#### Application Table

Displays all applications with the following columns: `ID`, `Business Name`, `Type`, `Status`, `Risk Score`.

Action buttons are role-aware:
- **Underwriters** do not see Evaluate / Premium / Issue buttons.
- **Agents & Admins** see all permitted actions.

---

### Rules Tab

> Accessible by **Underwriters** and **Admins**

#### Create Rule

Fill out all required fields and click **Add Rule**:

| Field | Description |
|---|---|
| `field` | The application field to evaluate |
| `operator` | Comparison operator (e.g. `>`, `<`, `==`) |
| `value` | The value to compare against |
| `riskPoints` | Positive integer — points added to risk score if rule matches |

#### Edit Rule

Update existing rule fields (if implemented). Available to Underwriters and Admins.

#### Delete Rule

Remove a rule from the system. Available to Underwriters and Admins.

---

## Validation & Error Handling

- **Form validation** — Applications and rules cannot be submitted with missing required fields.
- **Risk Points** — Must be a positive integer greater than 0.
- **Backend validation** — Returns `400` with structured error messages when required fields are absent.

  Example error response:
  ```json
  {
    "errors": {
      "Decision": ["The Decision field is required."]
    }
  }
  ```

- **Role-based access** — Tabs and action buttons are hidden or disabled based on the user's role.
- **Error feedback** — Alerts surface meaningful backend error messages to the user.

---

## Setup Instructions

**1. Clone the repository**
```bash
git clone <repo-url>
cd InsureFlow
```

**2. Install and start the frontend**
```bash
cd insureflow-frontend
npm install
npm start
```

**3. Start the backend API**
```bash
cd InsureFlow.API
dotnet run
```
The backend runs on `http://localhost:5002` by default.

**4. Open the app**

Navigate to [http://localhost:3000](http://localhost:3000) in your browser.

---

## Usage

1. **Log in** with Admin, Agent, or Underwriter credentials.
2. **Agents / Admins** — Use the Applications tab to create applications, evaluate risk, calculate premiums, and issue policies.
3. **Underwriters / Admins** — Use the Rules tab to add, edit, and delete underwriting rules.
4. Role restrictions are enforced automatically — unauthorized tabs and actions will not be visible.

---

## Test Credentials

To quickly test the application, use the following preconfigured accounts:

| Role          | Username       | Password       |
|---------------|----------------|----------------|
| Admin         | admin          | Password123    |
| Agent         | agent          | Password123    |
| Underwriter   | under          | Password123    |

> These accounts allow you to explore all role-specific functionality without creating new users.

---

## Notes

- `Decision` and `Risk Score` are computed entirely on the backend and are not manually entered.
- UI hides action buttons for roles that lack permission for those actions.
- Validation prevents incomplete or invalid data from being submitted.

**Planned Enhancements:**
- Full edit functionality for rules.
- Improved inline error display replacing browser alerts.
