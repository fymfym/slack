Feature: Validating the authenticated user

	In order to confirm basic details of the authenticated user
	I want to be able to execute a command that returns my authentication status

Scenario: Call the auth test API endpoint
	Given I am logged in to the Slack Web API as TabTest
	When I call auth.test
	Then I should receive an ok response matching:
	| ResponsePath | Value                           |
	| url          | https://tabslacktest.slack.com/ |
	| user         | tabtest                         |
	| team         | TAB Slack Test                  |
