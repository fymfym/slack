# To run this, first build the container:
# docker build .
#
# Then run the container:
# docker run --rm <containerid>
# 
# Or to run the container as a service:
# docker run -d <containerid>

FROM mono:4.0-onbuild

RUN mozroots --machine --import --sync
RUN yes | certmgr -ssl wss://slack-msgs.com:443

RUN mkdir plugins
RUN cp Tab.Slack.Bot.CoreHandlers.dll plugins/
RUN sed -i 's/<add key="slackbot\.plugindir" value="[^"]*" \/>/<add key="slackbot.plugindir" value="plugins" \/>/' Tab.Slack.Bot.ConsoleHost.exe.config

CMD [ "mono",  "./Tab.Slack.Bot.ConsoleHost.exe" ]
