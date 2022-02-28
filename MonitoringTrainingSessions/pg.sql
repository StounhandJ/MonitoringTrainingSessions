CREATE TABLE "roles" (
	"id" serial NOT NULL PRIMARY KEY,
	"name" VARCHAR(255) NOT NULL
);

CREATE TABLE "groups" (
	"id" serial NOT NULL PRIMARY KEY,
	"name" VARCHAR(255) NOT NULL
);

CREATE TABLE "sessions" (
	"id" serial NOT NULL PRIMARY KEY,
	"name" VARCHAR(255) NOT NULL
);

CREATE TABLE "users" (
	"id" serial NOT NULL PRIMARY KEY,
	"role_id" integer NOT NULL REFERENCES "roles" ("id"),
	"surname" VARCHAR(255) NOT NULL,
	"name" VARCHAR(255) NOT NULL,
	"patronymic" VARCHAR(255) DEFAULT '-',
	"login" VARCHAR(255) NOT NULL,
	"password" VARCHAR(255) NOT NULL
);

CREATE TABLE "schedule" (
	"id" serial NOT NULL PRIMARY KEY,
	"session_id" integer NOT NULL REFERENCES "sessions" ("id"),
	"group_id" integer NOT NULL REFERENCES "groups" ("id"),
	"number_day_week" smallint NOT NULL
);

CREATE TABLE "marks" (
	"id" serial NOT NULL PRIMARY KEY,
	"mark" smallint NOT NULL,
	"session_id" integer NOT NULL REFERENCES "sessions" ("id"),
	"who_put_user" integer NOT NULL REFERENCES "users" ("id"),
	"who_was_put_user" integer NOT NULL REFERENCES "users" ("id"),
	"date" timestamp NOT NULL
);

CREATE TABLE "user_groups"(
  "user_id" integer NOT NULL REFERENCES "users" ("id"),
  "group_id" integer NOT NULL REFERENCES "groups" ("id")
);

insert into roles("id", "name") values (1, 'teacher'),(2, 'student'),(3, 'admin')
