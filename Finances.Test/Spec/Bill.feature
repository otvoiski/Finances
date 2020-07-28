Feature: Bill

@mytag
Scenario: Add new bill
	Given have a bill
	When validate bill
	Then result should not be null;

Scenario: Add new bill but description is empty
	Given have a bill
	And description is empty
	When validate bill
	Then result should be null;

Scenario: Add new bill but date is null
	Given have a bill
	And date is null
	When validate bill
	Then result should be null;

Scenario: Add new bill but installment is zero
	Given have a bill
	And installment is 0
	When validate bill
	Then result should not be null;

Scenario: Add new bill but installment is less than zero
	Given have a bill
	And installment is less than -1
	When validate bill
	Then result should be null;

Scenario: Add new bill but value empty
	Given have a bill
	And value is empty
	When validate bill
	Then result should be null;

Scenario: Add new bill but type empty
	Given have a bill
	And type is empty
	When validate bill
	Then result should be null;

Scenario: Add new bill but is credit card type
	Given have a bill
	And type is credit card
	When validate bill
	Then result should not be null;	