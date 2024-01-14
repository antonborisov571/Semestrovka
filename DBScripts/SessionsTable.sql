create table if not exists Sessions(
	Id uuid primary key,
	Unlimited boolean not null default(false),
	AccountId int not null unique,
	Login varchar not null unique,
	CreateDateTime timestamp not null,
	Expires timestamp
)