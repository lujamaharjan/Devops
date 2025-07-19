

ansible webserver -m service -a "name=apache2 state=started enabled=true" -b
ansible webserver -m copy -a "src=./index.html dest=/var/www/html/index.html" -b

