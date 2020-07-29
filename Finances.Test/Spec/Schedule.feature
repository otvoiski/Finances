Feature: Schedule

@Schedule
Scenario: Add one schedule
	Given have one schedule
	When you access validation
	Then the result should not be null

Scenario: Add one schedule but schedule id is 0
	Given have one schedule
	And schedule id is 0
	When you access validation
	Then the result should not be null

Scenario: Add one schedule but bill id is zero
	Given have one schedule
	And bill id is 0
	When you access validation
	Then the result should not be null

Scenario: Add one schedule but bill id is less then zero
	Given have one schedule
	And bill id is -1
	When you access validation
	Then the result should be null

Scenario: Add one schedule but installment is empty
	Given have one schedule
	And installment is empty
	When you access validation
	Then the result should be null

Scenario: Add one schedule but month is empty
	Given have one schedule
	And month is empty
	When you access validation
	Then the result should be null

Scenario: Add one schedule but installment is not a number
	Given have one schedule
	And installment is not a number
	When you access validation
	Then the result should be null