CREATE TABLE "roles"
(
    "id"   serial       NOT NULL PRIMARY KEY,
    "name" VARCHAR(255) NOT NULL
);

CREATE TABLE "groups"
(
    "id"   serial       NOT NULL PRIMARY KEY,
    "name" VARCHAR(255) NOT NULL
);

CREATE TABLE "sessions"
(
    "id"   serial       NOT NULL PRIMARY KEY,
    "name" VARCHAR(255) NOT NULL
);

CREATE TABLE "users"
(
    "id"         serial       NOT NULL PRIMARY KEY,
    "role_id"    integer      NOT NULL REFERENCES "roles" ("id"),
    "surname"    VARCHAR(255) NOT NULL,
    "name"       VARCHAR(255) NOT NULL,
    "patronymic" VARCHAR(255) DEFAULT '-',
    "login"      VARCHAR(255) NOT NULL,
    "password"   VARCHAR(255) NOT NULL
);

CREATE TABLE "schedule"
(
    "id"              serial   NOT NULL PRIMARY KEY,
    "session_id"      integer  NOT NULL REFERENCES "sessions" ("id"),
    "group_id"        integer  NOT NULL REFERENCES "groups" ("id"),
    "number_day_week" smallint NOT NULL,
    "number_pair"     smallint NOT NULL
);

CREATE TABLE "marks"
(
    "id"               serial    NOT NULL PRIMARY KEY,
    "mark"             smallint  NOT NULL,
    "session_id"       integer   NOT NULL REFERENCES "sessions" ("id"),
    "who_put_user"     integer   NOT NULL REFERENCES "users" ("id"),
    "who_was_put_user" integer   NOT NULL REFERENCES "users" ("id"),
    "date"             timestamp NOT NULL
);

CREATE TABLE "user_groups"
(
    "user_id"  integer NOT NULL REFERENCES "users" ("id") ON DELETE CASCADE,
    "group_id" integer NOT NULL REFERENCES "groups" ("id")
);

CREATE TABLE "lesson"
(
    "id"          serial       NOT NULL PRIMARY KEY,
    "schedule_id" integer      NOT NULL REFERENCES "schedule" ("id") ON DELETE CASCADE,
    "topic"       VARCHAR(255) NOT NULL,
    "task"        text         NOT NULL,
    "discord_id"  VARCHAR(255) NOT NULL,
    "date"        date         NOT NULL
);

CREATE TABLE "time_schedule"
(
    "id"         serial  NOT NULL PRIMARY KEY,
    "number"     integer NOT NULL,
    "start_time" time    NOT NULL,
    "end_time"   time    NOT NULL
);



insert into roles("id", "name")
values (1, 'teacher'),
       (2, 'student'),
       (3, 'admin');

insert into time_schedule("number", "start_time", "end_time")
values (1, '8:30', '10:00'),
       (2, '10:10', '11:40'),
       (3, '12:00', '13:30'),
       (4, '14:00', '15:30'),
       (5, '15:40', '17:10');
