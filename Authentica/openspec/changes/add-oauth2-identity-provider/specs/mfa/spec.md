## ADDED Requirements

### Requirement: Email-based MFA Login
The system SHALL provide a POST /users/mfa/login/email endpoint for email-based multi-factor authentication.

#### Scenario: MFA code verification
- **WHEN** a user submits a valid MFA code during login
- **THEN** the system SHALL verify the code against the stored code
- **AND** complete the authentication process
- **AND** invalidate the used MFA code

#### Scenario: Invalid MFA code
- **WHEN** an invalid MFA code is submitted
- **THEN** the system SHALL return an authentication error
- **AND** increment failed MFA attempt counter
- **AND** potentially lock the account after multiple failures

#### Scenario: Expired MFA code
- **WHEN** an expired MFA code is submitted
- **THEN** the system SHALL return an error indicating the code has expired
- **AND** offer to send a new MFA code

### Requirement: Authenticator App MFA Login
The system SHALL provide a POST /users/mfa/login/authenticator endpoint for time-based one-time password authentication.

#### Scenario: TOTP verification
- **WHEN** a user submits a valid TOTP code from an authenticator app
- **THEN** the system SHALL verify the code using the shared secret
- **AND** complete the authentication process
- **AND** account for timezone and clock drift

#### Scenario: Replay attack prevention
- **WHEN** the same TOTP code is submitted twice
- **THEN** the system SHALL reject the second submission
- **AND** prevent replay attacks

#### Scenario: Clock drift handling
- **WHEN** TOTP codes are submitted with slight time differences
- **THEN** the system SHALL allow reasonable clock drift
- **AND** validate codes from adjacent time windows

### Requirement: MFA Settings Management
The system SHALL provide a POST /users/mfa/manage endpoint for users to manage their MFA settings.

#### Scenario: Enable MFA
- **WHEN** a user enables MFA with email-based method
- **THEN** the system SHALL configure email MFA for the account
- **AND** require verification of the new MFA method
- **AND** offer backup recovery codes

#### Scenario: Disable MFA
- **WHEN** a user disables MFA with proper authentication
- **THEN** the system SHALL remove MFA requirements from the account
- **AND** invalidate any remaining backup codes

#### Scenario: Switch MFA methods
- **WHEN** a user switches from email to authenticator app MFA
- **THEN** the system SHALL require setup of the new method
- **AND** only disable the old method after verification

### Requirement: Authenticator Management
The system SHALL provide a POST /users/mfa/manage/authenticator endpoint for managing authenticator app MFA.

#### Scenario: Setup authenticator
- **WHEN** a user sets up authenticator app MFA
- **THEN** the system SHALL generate a secret key and QR code
- **AND** provide backup codes for account recovery
- **AND** require verification of a workingTOTP code

#### Scenario: Regenerate backup codes
- **WHEN** a user requests new backup codes
- **THEN** the system SHALL invalidate old backup codes
- **AND** generate new random backup codes
- **AND** require reauthentication before providing codes

#### Scenario: Remove authenticator
- **WHEN** a user removes authenticator app as MFA method
- **THEN** the system SHALL require alternative authentication
- **AND** ensure the account has another MFA method or MFA is disabled

### Requirement: MFA Recovery Codes
The system SHALL provide a GET /users/mfa/recovery/codes endpoint for generating MFA recovery codes.

#### Scenario: Generate recovery codes
- **WHEN** an authenticated user requests recovery codes
- **THEN** the system SHALL generate unique single-use recovery codes
- **AND** display codes securely with warnings
- **AND** invalidate any existing recovery codes

#### Scenario: Limited code generation
- **WHEN** recovery codes have been generated recently
- **THEN** the system SHALL enforce a cooldown period
- **AND** require additional authentication before regeneration

### Requirement: MFA Recovery Code Redemption
The system SHALL provide a POST /users/mfa/recovery endpoint for redeeming MFA recovery codes.

#### Scenario: Redeem recovery code
- **WHEN** a user submits a valid recovery code
- **THEN** the system SHALL accept the code as MFA verification
- **AND** invalidate the used recovery code immediately
- **AND** warn the user to generate new codes

#### Scenario: Invalid recovery code
- **WHEN** an invalid recovery code is submitted
- **THEN** the system SHALL return an authentication error
- **AND** track failed attempts for security monitoring

#### Scenario: All recovery codes used
- **WHEN** all recovery codes for an account have been used
- **THEN** the system SHALL require normal MFA verification
- **AND** prompt the user to generate new recovery codes