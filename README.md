🧠 AI Note Processing System

Event-driven note management system with AI-powered summarization, built using .NET and Azure.

TBD: knowledge base for team collaboration, with auto tag generater and note search

🚀 Overview

This project demonstrates a scalable, event-driven backend system where user-created notes are processed asynchronously using AI.
Instead of processing requests synchronously, the system leverages message queues and background workers to improve performance and scalability.

⚙️ Tech Stack

	Backend: ASP.NET Core (.NET 8)
	Messaging: Azure Service Bus
	AI: Azure OpenAI
	Database: SQL Server / SQLite
	Real-time: SignalR
	Cloud: Azure Container Apps

✨ Features

	Create and manage notes
	AI-generated summaries
	Asynchronous processing via message queue
	Real-time updates using SignalR
	Secure authentication with JWT


🔄 Example Flow
	
	User creates a note
	API stores note and sends message to Service Bus
	Background worker processes the message
	AI generates summary
	Result is saved and pushed to client via SignalR

		
🛠 Getting Started

	Run locally
	git clone https://github.com/your-repo
	cd your-repo
	dotnet build
	dotnet run
