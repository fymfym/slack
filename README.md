# Slack RTM Bot and Web API Client

Projects:  
- The Slack RTM (WebSocket) Bot is designed for anyone to extend via extremely simple plugins.  
- The Slack Web API client library provides a simple way to integrate your code with Slack.

## Getting Started

### SlackBot  
```cs
var slackBot = SlackBot.Create("bot-api-key").Instantiate();

slackBot.Start();
```

### SlackApi  
```cs
var slackApi = SlackApi.Create("api-key");
```

### Things to note  
- You can run SlackBot without any plugins, but it obviously won't do much  
- Core bot state is provided via Tab.Slack.Bot.CoreHandlers - these also provide the best guidance for how you can create your own plugin message handlers  
- Core handlers are loaded by default, but you can build the bot using `WithoutCoreHandlers()` to disable them loading if so desired  
- You can specify individual core handlers using something like `WithoutCoreHandlers().WithPlugin<RtmStartHandler>()` for example   
- You can test the bot is running with core handlers properly by sending your bot a direct message "ping" - the bot should respond with "pong" 

## Todo
- [x] Flesh out the slack rest client code to handle full set of methods  
- [x] Sort out default bot handlers  
- [x] Add error/connection handling to bot  
- [x] Logging  
- [ ] Improve match extensions / logic  
- [ ] Add parsing code to translate IDs to names  
- [ ] Documentation / examples  
- [ ] Sort out dire test naming and layout (partially done)  
- [ ] Add some integration tests - potentially via a manual runner
- [ ] Add plugin config manager (with config file support for initial seeding)  
- [ ] Sort out Docker/Mono CI  
- [ ] Consider swapping out MEF with event based model and DI instead  
- [ ] Add those little status icon thingys to github page  
- [ ] Add scripting support (from @jachin84) - consider using Jint/ClearScript/PSHost/ScriptCS  
- [ ] Add auth config and control (from @jachin84)  
- [ ] Add support for webhooks (from @jachin84) - consider using Nancy  
- [ ] Create Docker/AMI/ARM images for instant virtualised deployments (from @jachin84)  
- [ ] Handle Retry-After header (https://api.slack.com/docs/rate-limits)  

