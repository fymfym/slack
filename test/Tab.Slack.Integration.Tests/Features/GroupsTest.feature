Feature: Managing team groups

	In order to manage team groups within Slack
	I want to be able to issue requests to create and control groups

Scenario: List all groups
	Given I am logged in to the Slack Web API as TabTest
	When I call groups.list with excludeArchived equal to True
	Then I should receive an ok response matching:
	| ResponsePath | RegEx |
	| xx           | xx    |
	When I call groups.list with excludeArchived equal to False
	Then I should receive an ok response matching:
	| ResponsePath | RegEx |
	| xx           | xx    |

Scenario: Set a group purpose
	Given I am logged in to the Slack Web API as TabTest
	When I call groups.setpurpose with:
	| Argument     | Value                 |
	| groupid      | TestGroup:id          |
	| grouppurpose | <new purpose SSS-SSS> |
	Then I should receive an ok response matching:
	| ResponsePath | RegEx |
	| xx           | xx    |

Scenario: Set a group topic
	Given I am logged in to the Slack Web API as TabTest
	When I call groups.settopic with:
	| Argument   | Value               |
	| groupid    | TestGroup:id        |
	| grouptopic | <new topic SSS-SSS> |
	Then I should receive an ok response matching:
	| ResponsePath | RegEx |
	| xx           | xx    |

Scenario: Gather information on a group
	Given I am logged in to the Slack Web API as TabTest
	When I call groups.info with groupid equal to TestGroup:id
	Then I should receive an ok response matching:
	| ResponsePath | RegEx |
	| xx           | xx    |

Scenario: Gather history about a group
	Given I am logged in to the Slack Web API as TabTest
	When I call groups.history with:
	| Argument     | Value        |
	| groupid      | TestGroup:id |
	| messagecount | 5            |
	# TODO: workout TS usage
	Then I should receive an ok response matching:
	| ResponsePath | RegEx |
	| xx           | xx    |

Scenario: Mark a group as read
	Given I am logged in to the Slack Web API as TabTest
	When I post a random message to TestGroup
	Then I should receive an ok response matching:
	| ResponsePath | RegEx |
	| xx           | xx    |
	When I call groups.mark with:
	| Argument  | Value        |
	| groupid   | TestGroup:id |
	| timestamp | <TSNOW>      |
	Then I should receive an ok response matching:
	| ResponsePath | RegEx |
	| xx           | xx    |

Scenario: Opening and closing groups
	Given I am logged in to the Slack Web API as TabTest
	And the group OpenGroup is closed
	When I call groups.open with groupid equal to OpenGroup:id
	Then I should receive an ok response matching:
	| ResponsePath | RegEx |
	| xx           | xx    |
	When I call groups.close with groupid equal to OpenGroup:id
	Then I should receive an ok response matching:
	| ResponsePath | RegEx |
	| xx           | xx    |

Scenario: Create a new group and manage it
	Given I am logged in to the Slack Web API as TabTest
	When I call groups.create with name equal to <createdgroupNNNNN>
	Then I should receive an ok response matching:
	| ResponsePath | RegEx |
	| xx           | xx    |
	When I call groups.rename with:
	| Argument  | Value                  |
	| groupid   | <createdgroupNNNNN>:id |
	| groupname | <renamedgroupNNNNN>:id |
	Then I should receive an ok response matching:
	| ResponsePath | RegEx                  |
	| groupname    | <renamedgroupNNNNN>:id |

Scenario: Invite and kick a user from a group
	Given I am logged in to the Slack Web API as TabTest
	And SampleUser is not in group TestGroup
	When I call groups.invite with:
	| Argument | Value         |
	| groupid  | TestGroup:id  |
	| userid   | SampleUser:id |
	Then I should receive an ok response matching:
	| ResponsePath | RegEx |
	| xx           | xx    |
	When I call groups.kick with:
	| Argument | Value         |
	| groupid  | TestGroup:id  |
	| userid   | SampleUser:id |
	Then I should receive an ok response matching:
	| ResponsePath | RegEx |
	| xx           | xx    |

