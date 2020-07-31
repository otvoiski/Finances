Feature: IsSchedule

@IsSchedule
Scenario: Invoice not scheduled and no installment.
	Given the bill
	And don't exists this description on schedule table
	And don't exists this bill vinculed in installments
	When check is schedule
	Then isSchedule should be true
	And error should be null

@IsSchedule
Scenario: Invoice not scheduled and have installment.
	Given the bill
	And don't exists this description on schedule table
	And exists this bill vinculed in installments
	When check is schedule
	Then isSchedule should be false
	And error message is "You cannot remove an invoice that is part of a installment!"

@IsSchedule
Scenario: Invoice scheduled and exist in bill table with different description and no installment.
	Given the bill
	And exists this description on schedule table
	And exists a after bills in bill table with different description
	When check is schedule
	Then isSchedule should be false
	And error message is "You cannot edit description if this is scheduled."

@IsSchedule
Scenario: Invoice scheduled and exist in bill table with iquals description and no installment.
	Given the bill
	And exists this description on schedule table
	And exists a after bills in bill table
	And exists this bill vinculed in installments
	When check is schedule
	Then isSchedule should be true
	And error should be null

@IsSchedule
Scenario: Invoice scheduled and exist in bill table with iquals description and no installment and delete flag.
	Given the bill
	And exists this description on schedule table
	And exists a after bills in bill table
	And exists this bill vinculed in installments
	When check is schedule with delete flag
	Then isSchedule should be false
	And error message is "You cannot remove an invoice that is part of a schedule!"

@IsSchedule
Scenario: Invoice scheduled and don't exist in bill table with iquals description and no installment.
	Given the bill
	And exists this description on schedule table
	And don't exists a after bills in bill table
	And exists this bill vinculed in installments
	When check is schedule
	Then isSchedule should be false
	And error message is "This invoice description already exists in on window of scheduling, change the description field!"