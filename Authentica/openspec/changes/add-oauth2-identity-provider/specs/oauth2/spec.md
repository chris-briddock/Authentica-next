## ADDED Requirements

### Requirement: OAuth2 Authorization Endpoint
The system SHALL provide a GET /oauth2/authorize endpoint that handles user authorization for OAuth 2.0 flows.

#### Scenario: Authorization code flow
- **WHEN** a client application redirects a user to /oauth2/authorize with valid parameters
- **THEN** the system SHALL display a login and consent screen
- **AND** upon successful authentication and consent, redirect to the client's redirect_uri with an authorization code

#### Scenario: Invalid client_id
- **WHEN** a request contains an invalid client_id
- **THEN** the system SHALL return an error response with appropriate error details

#### Scenario: Missing required parameters
- **WHEN** required parameters (client_id, redirect_uri, response_type) are missing
- **THEN** the system SHALL return an error indicating which parameters are missing

### Requirement: OAuth2 Token Endpoint
The system SHALL provide a POST /oauth2/token endpoint that issues OAuth 2.0 access tokens and refresh tokens.

#### Scenario: Authorization code exchange
- **WHEN** a client POSTS a valid authorization code with proper authentication
- **THEN** the system SHALL exchange the code for an access token and refresh token
- **AND** the response SHALL include token type, expires_in, and scope

#### Scenario: Client credentials grant
- **WHEN** a client authenticates using client credentials
- **THEN** the system SHALL issue an access token for the application
- **AND** the token SHALL have appropriate scopes for the application

#### Scenario: Refresh token grant
- **WHEN** a client presents a valid refresh token
- **THEN** the system SHALL issue a new access token
- **AND** optionally issue a new refresh token

#### Scenario: Invalid grant
- **WHEN** an invalid authorization code or refresh token is presented
- **THEN** the system SHALL return an invalid_grant error

### Requirement: OAuth2 Device Code Flow
The system SHALL provide a GET /oauth2/device endpoint that initiates the device code flow for devices without browsers.

#### Scenario: Device code initialization
- **WHEN** a client requests device authorization
- **THEN** the system SHALL create a device code and user code
- **AND** return device_code, user_code, verification_uri, and expires_in

#### Scenario: Device code polling
- **WHEN** the device polls the token endpoint with device_code
- **THEN** the system SHALL respond with authorization_pending until user completes authorization
- **AND** eventually return tokens upon user authorization

### Requirement: Token Validation and Security
The system SHALL validate all tokens and enforce OAuth 2.0 security requirements.

#### Scenario: Token validation
- **WHEN** a protected resource validates an access token
- **THEN** the system SHALL verify the token signature, expiration, and issuer
- **AND** return token metadata including scopes and user information

#### Scenario: Token revocation
- **WHEN** a token is revoked
- **THEN** the system SHALL immediately invalidate the token
- **AND** subsequent validation attempts SHALL fail