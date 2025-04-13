# ECB-ExchangeRateProvider

A simple ASP .NET Core Web API to create and handle user wallet balance.

## Overview
The Web API runs a periodic job using Quartz to update and store the latest currency rates, which can be used to retrieve a user's balance in different currencies and update a user's wallet balance using different stategies like: 
- Add Funds
- Substract Funds
- Substract Force Funds - which ignores if the amount to substract is larger than the current balance

## Additional
- `Rate Limiting:` The API supports server request limiting using the built in `AspNetCore.RateLimit` library.
- `Options Pattern:` Class/object binding to appSettings.json sections to keep everything clean and tidy
- `SQL MERGE:` Using the SQL `MERGE` statement to ensure fields mutate only when needed and in a single bulk query
- `Caching:` Using the in-memory cache to cache incoming rates every time they update, so we avoid making an HTTP call each time we want to retrieve the rates when we need them 
