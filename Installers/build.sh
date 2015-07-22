#!/bin/bash

mkdir temp

cp Linux/DEBIAN/. temp/DEBIAN/ -R

mkdir temp/tmp
cp Linux/spritefont.xml temp/tmp/spritefont.xml

mkdir temp/usr
mkdir temp/usr/share
mkdir temp/usr/share/icons
mkdir temp/usr/share/icons/gnome
mkdir temp/usr/share/icons/gnome/scalable
mkdir temp/usr/share/icons/gnome/scalable/mimetypes
cp Linux/sf-application.svg temp/usr/share/icons/gnome/scalable/mimetypes/sf-application.svg
cp Linux/sf-file.svg temp/usr/share/icons/gnome/scalable/mimetypes/sf-file.svg

mkdir temp/bin

cp ../SFEditor/SFEditor.Linux.Gtk3/bin/Release/sfeditor temp/bin/sfeditor
dpkg-deb --build temp sfeditor-gtk3.deb

rm temp/bin/sfeditor

cp ../SFEditor/SFEditor.Linux.Gtk2/bin/Release/sfeditor temp/bin/sfeditor
dpkg-deb --build temp sfeditor-gtk2.deb

rm -rf temp
