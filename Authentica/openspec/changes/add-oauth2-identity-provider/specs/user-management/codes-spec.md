## ADDED Requirements

### Requirement: MFA Code Sending
The system SHALL provide a POST /users/codes/mfa endpoint that sends multi-factor authentication codes.

#### Scenario: Send MFA code via email
- **WHEN** a user requests an MFA code with valid authentication
- **THEN** the system SHALL generate a time-limited MFA code
- **AND** send the code to the user's registered email address
- **AND** rate limit code generation attempts

#### Scenario: Rate limiting exceeded
- **WHEN** MFA code requests exceed rate limits
- **THEN** the system SHALL return a rate limit error
- **AND** inform the user when they can request another code

### Requirement: Password Reset Code Sending
The system SHALL provide a POST /users/codes/reset-password endpoint that sends password reset codes.

#### Scenario: Send password reset code
- **WHEN** a user requests a password reset with a valid email
- **THEN** the system SHALL generate a secure password reset code
- **AND** send the code to the user's email address
- **AND** invalidate any existing password reset codes for that user

#### Scenario: Email not found
- **WHEN** password reset is requested for an email that doesn't exist
- **THEN** the system SHALL return a generic success message
- **AND** not reveal whether the email exists for security

#### Scenario: Recent password reset request
- **WHEN** a password reset is requested too soon after a previous request
- **THEN** the system SHALL enforce a cooldown period
- **AND** inform the user to wait before requesting again

### Requirement: Email Confirmation Code Sending
The system SHALL provide a POST /users/codes/confirm-email endpoint that sends email confirmation codes.

#### Scenario: Resend confirmation code
- **WHEN** a user requests a new email confirmation code
- **THEN** the system SHALL generate a new confirmation code
- **AND** invalidate the previous confirmation code
- **AND** send the new code to the user's email

#### Scenario: Already confirmed email
- **WHEN** confirmation code is requested for an already confirmed email
- **THEN** the system SHALL indicate the email is already confirmed
- **AND** not send a new code

### Requirement: Email Update Code Sending
The system SHALL provide a POST /users/codes/update-email endpoint that sends email update confirmation codes.

#### Scenario: Send email update code
- **WHEN** an authenticated user requests to update their email
- **THEN** the system SHALL generate separate codes for old and new emails
- **AND** send confirmation codes to both email addresses
- **AND** require confirmation from both addresses for security

#### Scenario: Email in use by another account
- **WHEN** user tries to update to an email already used by another account
- **THEN** the system SHALL reject the update request
- **AND** indicate the email is already in use

### Requirement: Phone Number Update Code Sending
The system SHALL provide a POST /users/codes/update-phonenumber endpoint that sends phone number update codes.

#### Scenario: Send phone update code
- **WHEN** an authenticated user requests to update their phone number
- **THEN** the system SHALL generate a verification code
- **AND** send the code via SMS to the new phone number
- **AND** require confirmation before updating the phone number

#### Scenario: Invalid phone number format
- **WHEN** the phone number format is invalid
- **THEN** the system SHALL return a validation error
- **AND** provide format requirements for phone numbers