/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2000                    */
/* Created on:     2014/12/7 17:09:00                           */
/*==============================================================*/


if exists (select 1
            from  sysobjects
           where  id = object_id('CD_ORG_LEVEL')
            and   type = 'U')
   drop table CD_ORG_LEVEL
;

if exists (select 1
            from  sysindexes
           where  id    = object_id('CD_SX')
            and   name  = 'CD_SX_CODE_NAME'
            and   indid > 0
            and   indid < 255)
   drop index CD_SX.CD_SX_CODE_NAME
;

if exists (select 1
            from  sysobjects
           where  id = object_id('CD_SX')
            and   type = 'U')
   drop table CD_SX
;

if exists (select 1
            from  sysindexes
           where  id    = object_id('SYS_FUNCTION')
            and   name  = 'SYS_FUNCTION_FN_TREE_LAYER'
            and   indid > 0
            and   indid < 255)
   drop index SYS_FUNCTION.SYS_FUNCTION_FN_TREE_LAYER
;

if exists (select 1
            from  sysobjects
           where  id = object_id('SYS_FUNCTION')
            and   type = 'U')
   drop table SYS_FUNCTION
;

if exists (select 1
            from  sysindexes
           where  id    = object_id('SYS_ORGANIZATION')
            and   name  = 'SYS_ORGANIZATION_ORG_CODE'
            and   indid > 0
            and   indid < 255)
   drop index SYS_ORGANIZATION.SYS_ORGANIZATION_ORG_CODE
;

if exists (select 1
            from  sysindexes
           where  id    = object_id('SYS_ORGANIZATION')
            and   name  = 'SYS_ORGANIZATION_ORG_PARENT_ID'
            and   indid > 0
            and   indid < 255)
   drop index SYS_ORGANIZATION.SYS_ORGANIZATION_ORG_PARENT_ID
;

if exists (select 1
            from  sysindexes
           where  id    = object_id('SYS_ORGANIZATION')
            and   name  = 'SYS_ORGANIZATION_ORG_LAYER'
            and   indid > 0
            and   indid < 255)
   drop index SYS_ORGANIZATION.SYS_ORGANIZATION_ORG_LAYER
;

if exists (select 1
            from  sysindexes
           where  id    = object_id('SYS_ORGANIZATION')
            and   name  = 'SYS_ORGANIZATION_ORG_NAME'
            and   indid > 0
            and   indid < 255)
   drop index SYS_ORGANIZATION.SYS_ORGANIZATION_ORG_NAME
;

if exists (select 1
            from  sysobjects
           where  id = object_id('SYS_ORGANIZATION')
            and   type = 'U')
   drop table SYS_ORGANIZATION
;

if exists (select 1
            from  sysindexes
           where  id    = object_id('SYS_PART_FUNC')
            and   name  = 'SYS_PART_FUNC_PF_FN_ID'
            and   indid > 0
            and   indid < 255)
   drop index SYS_PART_FUNC.SYS_PART_FUNC_PF_FN_ID
;

if exists (select 1
            from  sysindexes
           where  id    = object_id('SYS_PART_FUNC')
            and   name  = 'SYS_PART_FUNC_PF_PART_ID'
            and   indid > 0
            and   indid < 255)
   drop index SYS_PART_FUNC.SYS_PART_FUNC_PF_PART_ID
;

if exists (select 1
            from  sysobjects
           where  id = object_id('SYS_PART_FUNC')
            and   type = 'U')
   drop table SYS_PART_FUNC
;

if exists (select 1
            from  sysindexes
           where  id    = object_id('SYS_PART_SUB_FUNC')
            and   name  = 'SYS_PART_SUB_FUNC_PSF_SF_ID'
            and   indid > 0
            and   indid < 255)
   drop index SYS_PART_SUB_FUNC.SYS_PART_SUB_FUNC_PSF_SF_ID
;

if exists (select 1
            from  sysindexes
           where  id    = object_id('SYS_PART_SUB_FUNC')
            and   name  = 'SYS_PART_SUB_FUNC_PART_ID'
            and   indid > 0
            and   indid < 255)
   drop index SYS_PART_SUB_FUNC.SYS_PART_SUB_FUNC_PART_ID
;

if exists (select 1
            from  sysobjects
           where  id = object_id('SYS_PART_SUB_FUNC')
            and   type = 'U')
   drop table SYS_PART_SUB_FUNC
;

if exists (select 1
            from  sysindexes
           where  id    = object_id('SYS_SUB_FUNC')
            and   name  = 'SYS_SUB_FUNC_SF_FN_ID'
            and   indid > 0
            and   indid < 255)
   drop index SYS_SUB_FUNC.SYS_SUB_FUNC_SF_FN_ID
;

if exists (select 1
            from  sysobjects
           where  id = object_id('SYS_SUB_FUNC')
            and   type = 'U')
   drop table SYS_SUB_FUNC
;

if exists (select 1
            from  sysindexes
           where  id    = object_id('UR_PART')
            and   name  = 'UR_PART_NAME'
            and   indid > 0
            and   indid < 255)
   drop index UR_PART.UR_PART_NAME
;

if exists (select 1
            from  sysobjects
           where  id = object_id('UR_PART')
            and   type = 'U')
   drop table UR_PART
;

if exists (select 1
            from  sysindexes
           where  id    = object_id('UR_USERS')
            and   name  = 'UR_USERS_USER_ORG_ID'
            and   indid > 0
            and   indid < 255)
   drop index UR_USERS.UR_USERS_USER_ORG_ID
;

if exists (select 1
            from  sysindexes
           where  id    = object_id('UR_USERS')
            and   name  = 'UR_USERS_USER_NAME'
            and   indid > 0
            and   indid < 255)
   drop index UR_USERS.UR_USERS_USER_NAME
;

if exists (select 1
            from  sysindexes
           where  id    = object_id('UR_USERS')
            and   name  = 'UR_USERS_USER_LOGIN_NAME'
            and   indid > 0
            and   indid < 255)
   drop index UR_USERS.UR_USERS_USER_LOGIN_NAME
;

if exists (select 1
            from  sysobjects
           where  id = object_id('UR_USERS')
            and   type = 'U')
   drop table UR_USERS
;

if exists (select 1
            from  sysobjects
           where  id = object_id('UR_USERS_PART')
            and   type = 'U')
   drop table UR_USERS_PART
;

/*==============================================================*/
/* Table: CD_ORG_LEVEL                                          */
/*==============================================================*/
create table CD_ORG_LEVEL (
   CODE_VALUE		varchar(2)	not null,
   CODE_NAME		varchar(100)	not null,
   CODE_PY		varchar(50)	null,
   CODE_DEL		smallint	null,
   CODE_SORT		int		null,
   constraint PK_CD_ORG_LEVEL primary key (CODE_VALUE)
)
;

/*==============================================================*/
/* Table: CD_SX                                                 */
/*==============================================================*/
create table CD_SX (
   CODE_VALUE		char(6)		not null,
   CODE_NAME		varchar(100)	not null,
   CODE_PY		varchar(50)	null,
   CODE_DEL		smallint	null,
   CODE_SORT		int		null,
   CODE_WX_PROVINCE	varchar(50)	null,
   CODE_WX_CITY		varchar(50)	null,
   constraint PK_CD_SX primary key (CODE_VALUE)
)
;

/*==============================================================*/
/* Index: CD_SX_CODE_NAME                                       */
/*==============================================================*/
create index CD_SX_CODE_NAME on CD_SX (
CODE_NAME ASC
)
;

/*==============================================================*/
/* Table: SYS_FUNCTION                                          */
/*==============================================================*/
create table SYS_FUNCTION (
   FN_ID                int                  not null,
   FN_SHORT_NAME        varchar(50)          null,
   FN_NAME              varchar(50)          not null,
   FN_URL               varchar(255)         null,
   FN_PARENT_ID         int                  not null,
   FN_IS_LEAF           smallint             not null,
   FN_TREE_LAYER        varchar(255)         not null,
   FN_REVERSE1          varchar(100)         null,
   FN_REVERSE2          varchar(200)         null,
   constraint PK_SYS_FUNCTION primary key nonclustered (FN_ID)
)
;

/*==============================================================*/
/* Index: SYS_FUNCTION_FN_TREE_LAYER                            */
/*==============================================================*/
create index SYS_FUNCTION_FN_TREE_LAYER on SYS_FUNCTION (
FN_TREE_LAYER ASC
)
;

/*==============================================================*/
/* Table: SYS_ORGANIZATION                                      */
/*==============================================================*/
create table SYS_ORGANIZATION (
   ORG_ID               varchar(64)          not null,
   ORG_NAME             varchar(100)         not null,
   ORG_CODE             varchar(64)          not null,
   ORG_CITY             char(6)              null,
   ORG_LEVEL            char(2)              null,
   ORG_LAYER            varchar(255)         null,
   ORG_PARENT_ID        varchar(64)          not null,
   ORG_IS_LEAF          smallint             null,
   ORG_MANAGER          varchar(50)          null,
   ORG_MANAGER_PHONE    varchar(50)          null,
   ORG_ADDRESS          varchar(255)         null,
   ORG_BILL_ADDRESS     varchar(255)         null,
   ORG_POST_CODE        varchar(10)          null,
   ORG_TELEPHONE        varchar(50)          null,
   ORG_UNITE            varchar(255)         null,
   ORG_ACTIVE           smallint             null,
   ORG_NOTE             varchar(500)         null,
   ORG_CREATE_ID        varchar(64)          null,
   ORG_CREATE_DATE      datetime             null,
   ORG_UPDATE_ID        varchar(64)          null,
   ORG_UPDATE_DATE      datetime             null,
   ORG_UNUSED1          varchar(200)         null,
   ORG_UNUSED2          varchar(200)         null,
   ORG_UNUSED3          int                  null,
   ORG_UNUSED4          int                  null,
   constraint PK_SYS_ORGANIZATION primary key nonclustered (ORG_ID)
)
;

/*==============================================================*/
/* Index: SYS_ORGANIZATION_ORG_NAME                             */
/*==============================================================*/
create index SYS_ORGANIZATION_ORG_NAME on SYS_ORGANIZATION (
ORG_NAME ASC
)
;

/*==============================================================*/
/* Index: SYS_ORGANIZATION_ORG_LAYER                            */
/*==============================================================*/
create index SYS_ORGANIZATION_ORG_LAYER on SYS_ORGANIZATION (
ORG_LAYER ASC
)
;

/*==============================================================*/
/* Index: SYS_ORGANIZATION_ORG_PARENT_ID                        */
/*==============================================================*/
create index SYS_ORGANIZATION_ORG_PARENT_ID on SYS_ORGANIZATION (
ORG_PARENT_ID ASC
)
;

/*==============================================================*/
/* Index: SYS_ORGANIZATION_ORG_CODE                             */
/*==============================================================*/
create unique index SYS_ORGANIZATION_ORG_CODE on SYS_ORGANIZATION (
ORG_CODE ASC
)
;

/*==============================================================*/
/* Table: SYS_PART_FUNC                                         */
/*==============================================================*/
create table SYS_PART_FUNC (
   PF_PART_ID           int                  not null,
   PF_FN_ID             int                  not null,
   PF_IS_FUNC           smallint             null,
   constraint PK_SYS_PART_FUNC primary key nonclustered (PF_PART_ID, PF_FN_ID)
)
;

/*==============================================================*/
/* Index: SYS_PART_FUNC_PF_PART_ID                              */
/*==============================================================*/
create index SYS_PART_FUNC_PF_PART_ID on SYS_PART_FUNC (
PF_PART_ID ASC
)
;

/*==============================================================*/
/* Index: SYS_PART_FUNC_PF_FN_ID                                */
/*==============================================================*/
create index SYS_PART_FUNC_PF_FN_ID on SYS_PART_FUNC (
PF_FN_ID ASC
)
;

/*==============================================================*/
/* Table: SYS_PART_SUB_FUNC                                     */
/*==============================================================*/
create table SYS_PART_SUB_FUNC (
   PSF_PART_ID          int                  not null,
   PSF_SF_ID            int                  not null,
   PSF_IS_FUNC          smallint             null,
   constraint PK_SYS_PART_SUB_FUNC primary key nonclustered (PSF_PART_ID, PSF_SF_ID)
)
;

/*==============================================================*/
/* Index: SYS_PART_SUB_FUNC_PART_ID                             */
/*==============================================================*/
create index SYS_PART_SUB_FUNC_PART_ID on SYS_PART_SUB_FUNC (
PSF_PART_ID ASC
)
;

/*==============================================================*/
/* Index: SYS_PART_SUB_FUNC_PSF_SF_ID                           */
/*==============================================================*/
create index SYS_PART_SUB_FUNC_PSF_SF_ID on SYS_PART_SUB_FUNC (
PSF_SF_ID ASC
)
;

/*==============================================================*/
/* Table: SYS_SUB_FUNC                                          */
/*==============================================================*/
create table SYS_SUB_FUNC (
   SF_ID                int                  not null,
   SF_FN_ID             int                  not null,
   SF_NAME_ID           varchar(50)          not null,
   SF_NAME              varchar(50)          not null,
   SF_POSITION          varchar(10)          null,
   SF_ICON              varchar(255)         null,
   SF_USE_KEY           smallint             null,
   SF_CONTENT           varchar(255)         null,
   SF_USE_MARCO         smallint             null,
   SF_CONFIRM_DATA      varchar(255)         null,
   SF_DIALOG_TITLE      varchar(255)         null,
   SF_INFO              varchar(255)         null,
   SF_ORDER             int                  null,
   SF_PAGE              varchar(10)          null,
   constraint PK_SYS_SUB_FUNC primary key nonclustered (SF_ID)
)
;

/*==============================================================*/
/* Index: SYS_SUB_FUNC_SF_FN_ID                                 */
/*==============================================================*/
create index SYS_SUB_FUNC_SF_FN_ID on SYS_SUB_FUNC (
SF_FN_ID ASC
)
;

/*==============================================================*/
/* Table: UR_PART                                               */
/*==============================================================*/
create table UR_PART (
   PART_ID              int                  not null,
   PART_NAME            varchar(100)         not null,
   PART_DESC            varchar(500)         null,
   constraint PK_UR_PART primary key nonclustered (PART_ID)
)
;

/*==============================================================*/
/* Index: UR_PART_NAME                                          */
/*==============================================================*/
create index UR_PART_NAME on UR_PART (
PART_NAME ASC
)
;

/*==============================================================*/
/* Table: UR_USERS                                              */
/*==============================================================*/
create table UR_USERS (
   USER_ID              varchar(64)          not null,
   USER_NAME            varchar(100)         not null,
   USER_ORG_ID          varchar(64)          null,
   USER_LOGIN_NAME      varchar(50)          not null,
   USER_LOGIN_PASSWD    varchar(50)          null,
   USER_LOGIN_DATE      datetime             null,
   USER_PHONE           varchar(50)          null,
   USER_MOBILE          varchar(50)          null,
   USER_EMAIL           varchar(100)         null,
   USER_WORK_NO         varchar(64)          null,
   USER_SEX             char(2)              null,
   USER_BIRTHDAY        datetime             null,
   USER_WORKED          varchar(255)         null,
   USER_EDUCATION       char(2)              null,
   USER_ORIGIN          char(6)              null,
   USER_TITLE           varchar(50)          null,
   USER_WORK_LIMIT      varchar(255)         null,
   USER_IDENT_NO        varchar(50)          null,
   USER_IN_DATE         datetime             null,
   USER_AREA            char(6)              null,
   USER_ADDRESS         varchar(255)         null,
   USER_POSTAL          varchar(20)          null,
   USER_ACTIVE          smallint             null,
   USER_OUT             smallint             null,
   USER_OUT_DATE        datetime             null,
   USER_NOTE            varchar(500)         null,
   USER_CREATE_ID       varchar(64)          not null,
   USER_CREATE_DATE     datetime             not null,
   USER_UPDATE_ID       varchar(64)          not null,
   USER_UPDATE_DATE     datetime             not null,
   USER_PASSWD_CHANGE_DATE datetime             null,
   USER_UNLOCK_DATE     datetime             null,
   USER_ADMIN           smallint             null,
   USER_UNUSED1         varchar(255)         null,
   USER_UNUSED2         varchar(255)         null,
   USER_UNUSED3         int                  null,
   USER_UNUSED4         int                  null,
   USER_BEFORE_NINE     smallint             null,
   USER_GATHER          int                  null,
   constraint PK_UR_USERS primary key nonclustered (USER_ID)
)
;

/*==============================================================*/
/* Index: UR_USERS_USER_LOGIN_NAME                              */
/*==============================================================*/
create unique index UR_USERS_USER_LOGIN_NAME on UR_USERS (
USER_LOGIN_NAME ASC
)
;

/*==============================================================*/
/* Index: UR_USERS_USER_NAME                                    */
/*==============================================================*/
create index UR_USERS_USER_NAME on UR_USERS (
USER_NAME ASC
)
;

/*==============================================================*/
/* Index: UR_USERS_USER_ORG_ID                                  */
/*==============================================================*/
create index UR_USERS_USER_ORG_ID on UR_USERS (
USER_ORG_ID ASC
)
;

/*==============================================================*/
/* Table: UR_USERS_PART                                         */
/*==============================================================*/
create table UR_USERS_PART (
   UP_USER_ID           varchar(64)          not null,
   UP_PART_ID           int                  not null,
   constraint PK_UR_USERS_PART primary key nonclustered (UP_USER_ID, UP_PART_ID)
)
;

