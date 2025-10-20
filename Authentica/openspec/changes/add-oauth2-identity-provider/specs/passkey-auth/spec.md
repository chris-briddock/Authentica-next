## ADDED Requirements

### Requirement: Passkey Attestation Options
The system SHALL provide a POST /users/passkeys/attestation/options endpoint that generates options for passkey registration.

#### Scenario: Generate attestation options
- **WHEN** an authenticated user requests to register a new passkey
- **THEN** the system SHALL generate WebAuthn attestation options
- **AND** include user information, challenge, and relying party details
- **AND** specify supported algorithms and credential parameters

#### Scenario: Unsupported device
- **WHEN** the requesting device doesn't support WebAuthn
- **THEN** the system SHALL return an error indicating WebAuthn required
- **AND** suggest alternative authentication methods

### Requirement: Passkey Registration (Attestation)
The system SHALL provide a POST /users/passkeys/attestation endpoint for registering new passkeys.

#### Scenario: Successful passkey registration
- **WHEN** a user submits a valid WebAuthn attestation response
- **THEN** the system SHALL verify the attestation and credential
- **AND** store the public key and metadata
- **AND** associate the passkey with the user account

#### Scenario: Invalid attestation
- **WHEN** the attestation response is invalid or malformed
- **THEN** the system SHALL return an error indicating registration failed
- **AND** provide information about why validation failed

#### Scenario: Duplicate passkey
- **WHEN** attempting to register a passkey that already exists
- **THEN** the system SHALL reject the duplicate registration
- **AND** allow the user to give the passkey a unique name

### Requirement: Passkey Assertion Options
The system SHALL provide a POST /users/passkeys/assertion/options endpoint that generates options for passkey authentication.

#### Scenario: Generate assertion options
- **WHEN** a user requests to authenticate with a passkey
- **THEN** the system SHALL generate WebAuthn assertion options
- **AND** include a challenge and allowCredentials for existing passkeys
- **AND** specify user verification requirements

#### Scenario: No passkeys available
- **WHEN** a user has no registered passkeys
- **THEN** the system SHALL return options without allowCredentials
- **AND** allow user to register a passkey or use alternative auth

### Requirement: Passkey Authentication (Assertion)
The system SHALL provide a POST /users/passkeys/assertion endpoint for authenticating with existing passkeys.

#### Scenario: Successful passkey authentication
- **WHEN** a user submits a valid WebAuthn assertion response
- **THEN** the system SHALL verify the assertion signature and challenge
- **AND** authenticate the user successfully
- **AND** create a new session and tokens

#### Scenario: Invalid assertion
- **WHEN** the assertion response is invalid
- **THEN** the system SHALL reject the authentication attempt
- **AND** increment failed authentication counter
- **AND** potentially require additional verification

#### Scenario: Passkey not found
- **WHEN** the asserted passkey ID doesn't exist for the user
- **THEN** the system SHALL reject the authentication
- **AND** offer to register the passkey if it's new

### Requirement: Passkey Security and Management
The system SHALL manage passkey security and lifecycle operations securely.

#### Scenario: Passkey revocation
- **WHEN** a user requests to remove a passkey
- **THEN** the system SHALL delete the passkey credentials
- **AND** prevent the passkey from being used for future authentication

#### Scenario: Lost device handling
- **WHEN** a user reports a lost device with passkeys
- **THEN** the system SHALL revoke all passkeys on that device
- **AND** require reauthentication before allowing new passkey registration

#### Scenario: Security audit
- **WHEN** reviewing passkey authentication events
- **THEN** the system SHALL maintain comprehensive audit logs
- **AND** track passkey creation, usage, and revocation events
- **AND** monitor for suspicious authentication patterns