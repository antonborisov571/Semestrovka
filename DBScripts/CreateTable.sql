create table if not exists Accounts(
	Id serial primary key,
	Login varchar not null unique,
	Email varchar not null unique,
	Password varchar not null,
	Salt varchar not null,
	DateRegistration date not null,
	LastOnline timestamp not null,
	Reputation int not null default(0),
	TractorName varchar,
	UrlImage varchar
);

create table if not exists Tractors(
	Id serial primary key,
	TractorName varchar not null,
	YearRelease date
);

create table if not exists Topics(
	Id serial primary key,
	UserId int not null,
	TopicName varchar not null,
	DateDispatch timestamp not null,
	Description varchar not null,
	constraint fk_account
      foreign key(UserId) 
	  references Accounts(Id)
);

create table if not exists Posts(
	Id serial primary key,
	UserId int not null,
	TopicId int not null,
	PostName varchar not null,
	DateDispatch timestamp not null,
	Description varchar not null,
	constraint fk_topic
      foreign key(TopicId) 
	  references Topics(Id),
	constraint fk_account
      foreign key(UserId) 
	  references Accounts(Id)
);

create table if not exists Messages(
	Id serial primary key,
	UserId int not null,
	PostId int not null,
	DateDispatch timestamp not null,
	TextMessage varchar,
	constraint fk_account
      foreign key(UserId) 
	  references Accounts(Id),
	constraint fk_topic
      foreign key(PostId) 
	  references Posts(Id)
);

create table if not exists Favourites(
	Id serial primary key,
	UserId int not null,
	MessageId int not null,
	DateDispatch timestamp not null,
	HasFavourite boolean not null default(false),
	constraint fk_user
      foreign key(UserId) 
	  references Accounts(Id),
	constraint fk_message
      foreign key(MessageId) 
	  references Messages(Id)
	  on delete cascade
);

create table if not exists Likes(
	Id serial primary key,
	UserId int not null,
	MessageId int not null,
	DateDispatch timestamp not null,
	HasLike boolean not null, 
	constraint fk_sender
      foreign key(UserId) 
	  references Accounts(Id),
	constraint fk_message
      foreign key(MessageId) 
	  references Messages(Id)
	  on delete cascade
);

create table if not exists Images(
	Id serial primary key,
	MessageId int not null,
	UrlImage varchar not null,
	constraint fk_message
      foreign key(MessageId) 
	  references Messages(Id)
	  on delete cascade
);

create table if not exists Jokes(
	Id serial primary key,
	TextJoke varchar not null,
	DateShow date not null
);