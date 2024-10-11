# Devblog & Portfolie Project

~ By Synx

## 📰 Table of content
* [Tags](#Tags)
* [Change Log](#Changelog)
* [Known Issues](#Issues)

## 📖 Intro
School project, made for a devblog and portfolie.
It will contain login system, signup system and so on, all in SSMS which will be added on a later date.
It can create, edit, delete and view posts.

## 🖥️ Information & tech

|           Language          | projectVersion | 
| --------------------------- | -------------- |
|    ASP.Net 8 & Transact-SQL |     v0.1.6     |
|-----------------------------|----------------|

## 🏷️ Tags 
[v0.1.6](https://github.com/SynxEU/Devblog-Portfolie/releases/tag/v0.1.5) \
[v0.1.5](https://github.com/SynxEU/Devblog-Portfolie/releases/tag/v0.1.5) \
[v0.1.3](https://github.com/SynxEU/Devblog-Portfolie/releases/tag/v0.1.3) \
[v0.1.1](https://github.com/SynxEU/Devblog-Portfolie/releases/tag/v0.1.1)

## 🧾 Changelog
[Github](https://github.com/SynxEU/Devblog-Portfolie/commits/master/)

## 🛑 Issues
Edit posts is currently disabled do to errors in modelstate \
Delete post (when you're on the post page) is currently disabled do to instance errors \
Edit profile doesn't redirect you to index, and therefore fails \
No error handling on person repo \
Tags are not working as intended and therefore disabled


## 📝 V0.0.0 til V1.0.0:

#### v0.1.6
* Fixed login, signup and createpost.
* Disabled tags (CreatePost) due to select menu not working.
* Updated the way i get post and authors from email to ids.
* Added SSMS file

#### v0.1.5
* Fixed some posts
* Converted person repo to SQL instead of CSV files 

#### v0.1.4
* Started converting to SQL
* Posts should be working with SSMS

#### v0.1.3
* Fixed a lot of design errors

#### v0.1.1
* Made some design changes
* Made delete for posts

#### v0.1.0
* Pages
* Login system
* Create & Read & Update for Posts
* Create & Update for Authors

#### v0.0.5
* Updated unit tests

#### v0.0.4
* Updated ReadMe

#### v0.0.35
* Edited ReadMe

#### v0.0.3
* Unit test

#### v0.0.2
* Services (Interfaces & Methods) 
* Dependency Injection 

#### v0.0.1
* Repositories 
* Models 

# 📜 To-Do

- [X] Backend
- [ ] Website & Design
- [ ] Database

# 📑 To-Do Website

- [ ] CRUD for Posts
- [ ] Create & Update for Authors 
- [ ] CRUD for Tags
- [X] Login system 
- [X] Password minimum lenght of 8 
- [X] Title max lenght 25 
- [X] Contect max Lenght of 5000 

# 📊 To-Do Database

- [X] Tables for Posts
- [X] Table for users
- [X] Table for tag
- [X] Joiner table for tag & posts (Not tested)
- [X] Stored procedures
- [X] Convert from CSV files to Database
- [ ] Implement it into Razor (60% done)
