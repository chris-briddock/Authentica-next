## ADDED Requirements

### Requirement: User Registration
The system SHALL provide a POST /users/register endpoint that allows new users to create accounts.

#### Scenario: Successful registration
- **WHEN** a valid registration request is submitted with email, password, and required fields
- **THEN** the system SHALL create a new user account
- **AND** send an email confirmation code to the user's email address
- **AND** return a success response indicating email confirmation is required

#### Scenario: Invalid email format
- **WHEN** registration request contains an invalid email format
- **THEN** the system SHALL return a validation error indicating the email format is invalid

#### Scenario: Weak password
- **WHEN** registration request contains a password that doesn't meet security requirements
- **THEN** the system SHALL return a validation error with password requirements

#### Scenario: Duplicate email
- **WHEN** registration request uses an email that already exists
- **THEN** the system SHALL return a conflict error indicating the email is already registered

### Requirement: User Login
The system SHALL provide a POST /users/login endpoint that authenticates users.

#### Scenario: Successful login
- **WHEN** valid email and password credentials are provided
- **THEN** the system SHALL authenticate the user
- **AND** return session tokens and user information
- **AND** create a new user session

#### Scenario: Invalid credentials
- **WHEN** invalid email or password is provided
- **THEN** the system SHALL return an authentication error
- **AND** increment failed login attempt counter

#### Scenario: Account locked
- **WHEN** login is attempted for an account that is locked
- **THEN** the system SHALL return an account locked error
- **AND** provide information about account recovery if available

#### Scenario: Unconfirmed email
- **WHEN** login is attempted for an account with unconfirmed email
- **THEN** the system SHALL require email confirmation before allowing login
- **AND** offer to resend confirmation email

### Requirement: User Logout
The system SHALL provide a POST /users/logout endpoint that terminates user sessions.

#### Scenario: Successful logout
- **WHEN** a logout request is made with valid session tokens
- **THEN** the system SHALL invalidate the session tokens
- **AND** remove the user session from active sessions

#### Scenario: Logout from all devices
- **WHEN** a logout request includes "all_devices" parameter
- **THEN** the system SHALL invalidate all active sessions for the user
- **AND** return confirmation of all sessions terminated

### Requirement: User Retrieval
The system SHALL provide a GET /users endpoint that retrieves authenticated user details.

#### Scenario: Retrieve current user
- **WHEN** an authenticated user requests their own details
- **THEN** the system SHALL return the user's profile information
- **AND** exclude sensitive information like password hashes

#### Scenario: Invalid authentication
- **WHEN** an unauthenticated request is made to retrieve user details
- **THEN** the system SHALL return an authentication required error

### Requirement: User Deletion
The system SHALL provide a DELETE /users/delete endpoint that permanently removes user accounts.

#### Scenario: Successful deletion
- **WHEN** an authenticated user requests account deletion with proper confirmation
- **THEN** the system SHALL permanently delete the user account
- **AND** remove all associated sessions and tokens
- **AND** handle data retention requirements

#### Scenario: Deletion without confirmation
- **WHEN** deletion is requested without proper confirmation
- **THEN** the system SHALL require explicit confirmation before proceeding
- **AND** inform about data permanence

### Requirement: Email Confirmation
The system SHALL provide a POST /users/confirm-email endpoint that validates email addresses.

#### Scenario: Successful confirmation
- **WHEN** a valid confirmation code is submitted
- **THEN** the system SHALL mark the user's email as confirmed
- **AND** activate the user account if it was pending confirmation

#### Scenario: Invalid confirmation code
- **WHEN** an invalid or expired confirmation code is submitted
- **THEN** the system SHALL return an error indicating the code is invalid
- **AND** offer to send a new confirmation code

### Requirement: Password Reset
The system SHALL provide a POST /users/reset-password endpoint that allows users to reset forgotten passwords.

#### Scenario: Reset with valid code
- **WHEN** a valid password reset code and new password are submitted
- **THEN** the system SHALL update the user's password
- **AND** invalidate all existing sessions for security
- **AND** require re-login with new password

#### Scenario: Invalid reset code
- **WHEN** an invalid or expired reset code is submitted
- **THEN** the system SHALL return an error indicating the code is invalid

#### Scenario: Weak new password
- **WHEN** the new password doesn't meet security requirements
- **THEN** the system SHALL return a validation error with password requirements