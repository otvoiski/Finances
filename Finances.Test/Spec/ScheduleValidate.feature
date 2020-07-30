Feature: Schedule Validate

@ScheduleValidate
Scenario: Add one schedule
	Given have one schedule
	When you access validation
	Then the result should not be null

Scenario: Add one schedule but schedule id is 0
	Given have one schedule
	And schedule id is 0
	When you access validation
	Then the result should not be null

Scenario: Add one schedule but description is empty
	Given have one schedule
	And schedule description is empty
	When you access validation
	Then the result should be null

Scenario: Add one schedule but price is empty
	Given have one schedule
	And price is empty
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