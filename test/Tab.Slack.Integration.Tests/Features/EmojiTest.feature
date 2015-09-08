Feature: Retrieve a list of emojis

	In order to view which emojis are available for use
	I want to be able to request a list of all emojis recognised for the team

Scenario: Call the emoji list API endpoint
	Given I am logged in to the Slack Web API as TabTest
	When I call emoji.list
	Then I should receive an ok response matching:
	| ResponsePath        | RegEx      |
	| Emoji[simple_smile] | https.+png |
	| Emoji[rage1]        | https.+png |
