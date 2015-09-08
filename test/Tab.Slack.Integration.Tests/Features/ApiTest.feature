Feature: Testing the Slack API

	In order to confirm that the API will accept my requests
	I want to be able to execute a test command and get a response

Scenario: Call the test API endpoint with no args
	Given I am logged in to the Slack Web API as TabTest
	When I call api.test
	Then I should receive an ok response

Scenario: Call the test API endpoint with basic args
	Given I am logged in to the Slack Web API as TabTest
	When I call api.test with args equal to foo, bar
	Then I should receive an ok response matching:
	| ResponsePath | Value |
	| args[arg1]   | foo   |
	| args[arg2]   | bar   |

Scenario: Call the test API endpoint with an error flag
	Given I am logged in to the Slack Web API as TabTest
	When I call api.test with:
	| Argument | Value |
	| error    | uhoh  |
	| args     | foo   |
	Then I should receive a response matching:
	| ResponsePath | Value |
	| ok           | False |
	| error        | uhoh  |
	| args[arg1]   | foo   |
	| args[error]  | uhoh  |
