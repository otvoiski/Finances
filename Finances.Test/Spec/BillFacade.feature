Feature: BillFacade

@BillFacade
Scenario: Add one bill
	Given a bill
	And possible insert on bill database
	When you save bill
	Then result should be true

Scenario: Add one bill with installment
	Given a bill
	And possible insert on bill database
	And have 2 installments
	And type is credit card
	When you save bill
	Then result should be true

Scenario: Add one bill but is paid
	Given a bill
	And is paid
	And possible insert on bill database
	When you save bill
	Then result should be true