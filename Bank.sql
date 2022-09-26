use Bank

create table Customer(
accountNumber int UNIQUE NOT NULL,
checkingAccNumber int,
savingsAccNumber int,
firstName varchar(40) not null,
lastName varchar(40) not null,
userName varchar(40) not null,
accPassword varchar(20) not null,
email varchar(40) not null,
isAdmin bit,

constraint fk_checkingAccNumber foreign key(checkingAccNumber) references CheckingAccount,
constraint fk_savingsAccNumber foreign key(savingsAccNumber) references SavingsAccount,

)

create table CheckingAccount(
checkingAccNumber int unique not null,
accBalance money,
transaction_1 varChar(40),
transaction_2 varChar(40),
transaction_3 varChar(40),
transaction_4 varChar(40),
transaction_5 varChar(40),

CONSTRAINT CHK_CheckingBalance CHECK (accBalance >= 0)
)
ALTER table SavingsAccount 
ADD PRIMARY KEY (savingsAccNumber)
create table SavingsAccount(
savingsAccNumber int unique not null,
accBalance money,
transaction_1 varChar(40),
transaction_2 varChar(40),
transaction_3 varChar(40),
transaction_4 varChar(40),
transaction_5 varChar(40),

CONSTRAINT CHK_SavingsBalance CHECK (accBalance >= 0)
)

insert into Customer values(3, null, null, 'chris', 'rudder', 'customer', 'devils', 'email', 0)
select * from Customer
create table CheckingAccount(
checkingAccID int primary key,
balance money,
CheckingIsActive bit,


)

select count(*) from Customer where userName = 'cbrudder' and accPassword = 'devils88'

ALTER TABLE SavingsAccount
drop constraint 
ADD CONSTRAINT df_savingsIsActive
DEFAULT false FOR savingsIsActive


ADD CONSTRAINT df_savingsTransaction_5
DEFAULT 'N/A' FOR transaction_5
							
update SavingsAccount set SavingsIsActive = 1 where savingsAccNumber = 5


select * from CheckingAccount
select * from SavingsAccount
2 4 890
alter table SavingsAccount;

delete from CheckingAccount where checkingAccNumber=2
delete from SavingsAccount where savingsAccNumber=2

alter table Customer add constraint chk_email check (email like '%_@__%.__%')
alter table Customer drop constraint chk_email
alter table SavingsAccount
update SavingsAccount set SavingsIsActive=1 where savingsAccNumber=3 
select * from Customer
update Customer set checkingAccNumber=3, savingsAccNumber=3 where accountNumber=3

