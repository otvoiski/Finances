Feature: LoadSchedue

@LoadSchedue
Scenario: loading schedules
	Given list of 5 existent schedules
	And with today is "07/30/2020"
	And price on schedule of -12,32
	And not exist this on table bill
	And possible insert on database
	When the call scheduling list is not null
	And access load schedule
	Then the result should be true

Scenario: loading schedules but exist bill
	Given list of 5 existent schedules
	And with today is "07/30/2020"
	And price on schedule of -12,32
	And exist this on table bill
	And possible insert on database
	When the call scheduling list is not null
	And access load schedule
	Then the result should be true

Scenario: loading schedules but cannot insert bill
	Given list of 5 existent schedules
	And with today is "07/30/2020"
	And price on schedule of -12,32
	And not exist this on table bill
	And not possible insert on database
	When the call scheduling list is not null
	And access load schedule
	Then the result should be false
