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

CMD [ "mono",  "./Tab.Slack.Bot.ConsoleHost.exe" ]
