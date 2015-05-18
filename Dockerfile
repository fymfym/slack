FROM mono:4.0-onbuild

RUN mozroots --machine --import --sync
RUN yes | certmgr -ssl wss://slack-msgs.com:443

CMD [ "mono",  "./Tab.Slack.Bot.ConsoleHost.exe" ]
