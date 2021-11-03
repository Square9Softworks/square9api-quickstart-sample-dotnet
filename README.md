# Square 9 API Sample .NET Consumer

This project is intended to give those developers consuming our API an example of some best-practice in completing work when dealing with license tokens.

## Best practices for service license token usage

When processing volumes of work in the context of something like a service worker; it is considered best practice to
reuse a long-lived token, rather than to request and dispose of a license token for each request. In
simple terms, this can reduce each transaction from 3 calls to 1. Depending on your flow, this can greatly reduce the load on the SQL
database by preventing additional queries necessary by the create license process.

### Example API Consumer Service Workflow:
1. **Ensure we have a token:**
    1. Load our stored license token (if we have one).
    2. If we do not have one, request a new one and save it for future use.
2. **Do any work** that needs to be completed that requires a token.
3. **If we are unable to complete work because our license is expired:**
    1. Remove invalid stored token.
    2. Restart the flow, if we have not failed too many times already.

## What API calls does this project use?

This project completes three API requests:
1. Get License Token
2. Get Database List
3. Return License Token (set inactive)

## Requirements

- .NET 5
- GlobalSearch 6.0+
