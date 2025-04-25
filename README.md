# Hacker News API (ASP.NET Core)

## Description
This is a backend API built with **ASP.NET Core** that fetches stories from Hacker News and provides the functionality for **pagination**, **search**, and **story details**. The API allows you to get the total count of stories, view specific stories by ID, and list stories with filtering and pagination.

## Features
- Fetches the latest stories from **Hacker News API**.
- Supports **pagination** and **search** functionality.
- Provides detailed information for each story, including title, score, author, and article link.
- Implements **caching** for optimizing frequently requested data.

## Technologies
- **ASP.NET Core** (for building the Web API)
- **HttpClient** (for interacting with the Hacker News API)
- **MemoryCache** (for caching the list of story IDs)
- **Swagger** (for API documentation)

## Installation

### 1. Clone the repository
Clone the repository to your local machine using Git:

```bash
git clone https://github.com/<your-username>/hackernews-api.git
