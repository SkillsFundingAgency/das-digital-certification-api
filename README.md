## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# SFA.DAS.DigitalCertificates.Api

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">


[![Build Status](https://dev.azure.com/sfa-gov-uk/Digital%20Apprenticeship%20Service/_apis/build/status/_projectname_?branchName=master)](https://dev.azure.com/sfa-gov-uk/Digital%20Apprenticeship%20Service/_build/latest?definitionId=_projectid_&branchName=master)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=_projectId_&metric=alert_status)](https://sonarcloud.io/dashboard?id=_projectId_)
[![Jira Project](https://img.shields.io/badge/Jira-Project-blue)](https://skillsfundingagency.atlassian.net/secure/RapidBoard.jspa?rapidView=564&projectKey=_projectKey_)
[![Confluence Project](https://img.shields.io/badge/Confluence-Project-blue)](https://skillsfundingagency.atlassian.net/wiki/spaces/_pageurl_)
[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

```
The Digital Certificates API contains functionality to allow apprentices to request digital and print certificates.
```

## How It Works

```
The Digital Certificates service consists of a database, Inner API, Outer API's, Apprentice Portal Web and Jobs, this repo is the Inner API.
```

## 🚀 Installation

### Pre-Requisites

```
* A clone of this repository
* A code editor that supports Azure functions and .NetCore 8.0 e.g. Visual Studio 2022
* A SQL server which is either an Azure DB or is Azure DB compatible e.g. SQL Server 2022 Developer Edition running locally
* An Azure Service Bus instance (Only required for the API when sending emails)
```

```
Publish the database to Azure or a local instance by selecting SFA.DAS.DigitalCertificates.Database and 
selecting Publish, no additional properties need to be specified.
```

### Config

Azure Table Storage config

Row Key: SFA.DAS.DigitalCertificates.Api_1.0

Partition Key: LOCAL

```
This utility uses the standard Apprenticeship Service configuration. All configuration can be found in the [das-employer-config repository]
(https://github.com/SkillsFundingAgency/das-employer-config).
```

## 🔗 External Dependencies

```
* None
```

## Technologies

```
* .NetCore 8.0
* Azure Functions V4
* Azure Table Storage
* NUnit
* Moq
* FluentAssertions
```

## 🐛 Known Issues

```
* None
```