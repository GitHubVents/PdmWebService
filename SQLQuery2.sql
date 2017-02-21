update TaskInstance 
set TaskStatus = 3
where TaskStatus in (2,4)


select * from TaskSelection where TaskInstanceID = 1550;