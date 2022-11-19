# Battle Assistant

## Summary
This is a project using Microsofts WinUI3 framework to create a application that supports the Combat Mission games play by email system. This is achieved by automating the process of moving files from the incoming email folder and outgoing email folder of a combat mission game and the shared drive of the opponent. This project took inspiration from Combat Mission Helper in what initial features to add.

![Games Page Image](https://user-images.githubusercontent.com/94839295/175392448-bdb0f9dc-3658-444c-95e8-489fe7a0c355.png)

## Installation Instructions
YOU NEED WINDOWS 10 OR 11 TO USE THIS APPLICATION
1. Go to the latest release and download the .zip folder called Battle Assitant_x.x.x_x64.zip.
2. Unzip the folder anywhere on your computer.
3. For first time installation you need to install the certificate, if your updating you don't need to install the certificate again.
4. Double click on the .msxi file to install the application

##Installing the certificate
1. Double click on the .cer file and install it to you computer, ensure you install it to your local machine instead of current user. 
2. If there is a page asking for a password for the private key, ignore it and click next.
3. When selecting where to store the certificate, you need to select place all certificates in the following store and then browse to the Trusted People folder.
4. Click Finish.

## Dev Installation Instructions
1. Clone the git repo onto your computer and modify the code you want to change.
2. Create a pull request with the modified code and explain why the code changes have been made. If it fixes an issue add the issue into the description of the pull request.

## Features
- A modern nice looking UI.
- The main automation loop for file moving.
- Auto clean folders as battle progresses.
- Clean all folders when battle has ended.
- The ability to manage mulitple battles with multiple opponents in multiple combat mission games.

## Why this project was made
This project was mainly made for my own benefit as I wanted an excuse to learn C# and how to make a modern desktop application. I choose this specific idea as Combat Mission Helper isn't being developed anymore and I thought there were areas that could be improved upon. 
