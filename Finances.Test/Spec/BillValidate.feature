Feature: BillValidate

@BillValidate
Scenario: Add new bill
	Given a data bill
	When validate bill
	Then result should not be null;

Scenario Outline: Add new bill but description is empty
	Given a data bill
	And bill description is empty
	When validate bill
	Then result should be null;

Scenario: Add new bill but date is null
	Given a data bill
	And date is null
	When validate bill
	Then result should be null;

Scenario: Add new bill but installment is zero
	Given a data bill
	And installment is 0
	When validate bill
	Then result should not be null;

Scenario: Add new bill but value empty
	Given a data bill
	And value is empty
	When validate bill
	Then result should be null;

Scenario: Add new bill but type empty
	Given a data bill
	And type is empty
	When validate bill
	Then result should be null;

Scenario: Add new bill but is credit card type
	Given a data bill
	And data type is credit card
	When validate bill
	Then result should not be null;

Scenario: Add new bill but is paid
	Given a data bill
	And data is paid
	When validate bill
	Then result should not be null;