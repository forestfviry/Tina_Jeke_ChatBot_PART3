CyberGuard Chatbot System
A responsive Windows Presentation Foundation (WPF) desktop assistant engineered to deliver interactive cybersecurity awareness and sentiment-aware user support. The application filters conversational natural language input, isolates core security intents, dynamically evaluates user emotional states, and couples targeted digital perimeter guidance with real-time empathy processing.

Technical Architecture Overview
The system is constructed using a decoupled, file-by-file model layer layout to enforce a strict separation of concerns between raw asset data storage, logical state calculation, and the front-end presentation surface.

1. Presentation Interface (MainWindow.xaml)
The user interface layer is built entirely via a responsive XAML layout grid featuring three distinct operational viewports:

logo_grid: An initialization splash screen displaying a custom, auto-generated system asset matrix using high-density ASCII layout streams.

username_grid: A secure agent registration panel designed to capture and pass user identity tokens downstream.

chats_grid: The primary conversational interface housing a scrolling thread log, input terminal, and operational action triggers.

The layout is wrapped in a high-contrast dark mode theme utilizing a deep charcoal canvas background matched with cyber-blue interactive accents for enhanced visual accessibility.

2. View Controller (MainWindow.xaml.cs)
The code-behind orchestration class manages the application thread execution path, event hooks, and interface routing loops:

Event Loop Key Binding: The input terminal intercepts keyboard states directly within the input event handlers. By configuring submittal buttons with active tracking defaults, users can fire data streams instantly using the keyboard Enter key.

Boundary Validation: Implements explicit validation trapping frameworks. Unmapped string streams or unrecognized inputs are intercepted safely before executing matching routines, logging a friendly fallback string to prevent thread locks or crashes.

3. Logic Engine (SentimentDetector.cs)
The core processing brain evaluates raw text payloads to determine user intents and emotional states:

Memory Optimization: Leverages strongly typed, generic collections rather than legacy array lists to minimize resource consumption during string tracking and sorting routines.

Lookback Context Tracking: Monitors state history shifts across multi-turn interactions. If a user exhibits a sudden emotional fluctuation, the engine safely alters string concatenation pathways without dropping technical context.

4. Data Repository (respond.cs)
Acts as the central static dictionary asset for the application knowledge base:

Noise Filtering: Seeds a massive collection of standard English stop words used to strip away grammatical noise, allowing the evaluation loops to isolate true search intents.

Intent Categorisation: Maps structured text strings using an explicit intent-prefix convention. Technical parameters are cataloged under specific domains such as phishing, firewalls, and virtual private networks, while the empathy matrix seeds distinct conversational variations for human emotional states.

Local Compilation and Deployment
System Prerequisites
Microsoft Visual Studio (Community, Professional, or Enterprise editions)

.NET Framework SDK development tools

Local media hardware (for audio greeting playback assets)

Installation Steps
Clone or download the repository files to your local workstation.

Ensure that your core system files (MainWindow.xaml, MainWindow.xaml.cs, SentimentDetector.cs, and respond.cs) are positioned within the root project workspace folder.

Place your target bitmap image named logo.jpg and your audio greeting asset named Soundd.wav directly into your build output directory to resolve local file path lookups.

Open the solution file within Visual Studio.

Press F5 or select the green Start button on the top menu bar to build, compile, and run the desktop application live.
