# Sports Store Application

## Description

The `SportsStore` application is a comprehensive e-commerce solution that demonstrates modern web development practices using .NET 8. This application follows the classic architecture pattern commonly used by online stores worldwide.

## Application Features

Upon completing this task, you will have a fully functional application that implements the following features:
- An online product catalog where customers can browse products by category and page
- A shopping cart system where users can add and remove products
- A checkout process where customers can enter delivery details
- An administration area with full CRUD (Create, Read, Update, Delete) capabilities for managing products and orders

## Technology Stack

This project utilizes the .NET 8 platform with the following tools and frameworks:
- .NET 8 SDK
- Entity Framework Core
- SQL Server
- ASP.NET Core MVC

## Development Environment

**Visual Studio 2022** is the recommended IDE for this project, as it provides the most convenient development experience. However, if your development machine doesn't have Visual Studio 2022 configured, you can use:
- Visual Studio Code with the .NET 8 SDK
- Any other IDE that supports .NET development

## Project Structure

The development process is divided into five sequential steps, each building upon the previous one. Each step has its own feature branch with detailed documentation:

| Step | Description | Feature Branch Name |
|------|-------------|-------------------|
| 1. | Building the basic infrastructure for the SportsStore application | step-1 |
| 2. | Creating a simple domain model with a product repository supported by SQL Server and Entity Framework Core. Developing the HomeController for paginated product lists. Implementing clean URL schemes and basic styling | step-2 |
| 3. | Implementing category-based navigation and basic shopping cart functionality | step-3 |
| 4. | Completing the shopping cart system with a comprehensive checkout process | step-4 |
| 5. | Implementing full CRUD operations for product management and order processing capabilities | step-5 |

After completing all steps, you will have the final version of the application in the `main` branch.

## Project Configuration

Each project file in the solution must be properly configured:

### 1. PropertyGroup Configuration
Your `<PropertyGroup>` section should contain the following lines:
```xml
<TargetFramework>net8.0</TargetFramework>
<EnableNETAnalyzers>true</EnableNETAnalyzers>
<AnalysisMode>AllEnabledByDefault</AnalysisMode>
```

### 2. EditorConfig Configuration
The project includes a comprehensive `.editorconfig` file that automatically configures:
- Code style conventions
- Naming conventions
- Formatting rules
- Analyzer rule severities
- C# specific settings

This configuration is automatically applied to all C# files in the project.

## Important: Independent Problem Solving and Critical Thinking

This repository intentionally contains minor inaccuracies, typos, incomplete hints, and occasionally conflicting instructions. Some branches may also introduce merge conflicts by design. These situations are part of the learning experience and are meant to simulate real-world development challenges.

Before asking a mentor or SME for help, you are expected to:

* Carefully read the full task description and related files.
* Analyze error messages, logs, and compiler output.
* Compare differences between branches and resolve merge conflicts independently.
* Consult official documentation, course materials, and previous tasks.
* Propose your own solution or hypothesis before escalating a question.

**Remember**: the goal of this project is not only to implement features but also to develop problem-solving skills, debugging habits, and engineering independence. Questions that demonstrate prior investigation and clear reasoning will receive faster and more detailed feedback.

## Reference

*This task was developed based on examples from the book [Adam Freeman: Pro ASP.NET Core 7, Tenth Edition](https://www.amazon.com/Pro-ASP-NET-Core-7-Tenth/dp/1633437825). For additional guidance or if you encounter difficulties, please refer to the source material.*  