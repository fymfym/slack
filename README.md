# Slack RTM Bot and Web API Client

Projects:  
- The Slack RTM (WebSocket) Bot is designed for anyone to extend via extremely simple plugins.  
- The Slack Web API client library provides a simple way to integrate your code with Slack.

## Getting Started

### SlackBot  
```cs
var slackBot = SlackBot.Build("bot-api-key").Instantiate();

slackBot.Start();
```

### SlackApi  
```cs
var slackApi = new SlackApi("api-key");
```

### Things to note  
- You can run SlackBot without any plugins, but it obviously won't do much  
- Core bot state is provided via Tab.Slack.Bot.CoreHandlers - these also provide the best guidance for how you can create your own plugin message handlers  
- Core handlers are loaded by default, but you can build the bot using `WithoutCoreHandlers()` to disable them loading if so desired  
- You can specify individual core handlers using something like `WithoutCoreHandlers().WithPlugin<RtmStartHandler>()` for example   
- You can test the bot is running with core handlers properly by sending your bot a direct message "ping" - the bot should respond with "pong" 

## Todo
- ~~Flesh out the slack rest client code to handle full set of methods~~  
- ~~Sort out default bot handlers~~  
- Add error/connection handling to bot  
- Logging  
- Documentation / examples  
- Sort out dire test naming and layout  
- Add some integration tests - potentially via a manual runner
- Add plugin config manager
