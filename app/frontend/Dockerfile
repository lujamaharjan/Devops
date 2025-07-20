FROM nginx:alpine

RUN rm -rf /usr/share/nginx/html/*
COPY . /usr/share/nginx/html


RUN adduser -D -s /bin/sh nginxuser
RUN chown -R nginxuser:nginxuser /usr/share/nginx/html
RUN chown -R nginxuser:nginxuser /usr/share/nginx/html 
RUN mkdir -p /var/cache/nginx/client_temp \
RUN chown -R nginxuser:nginxuser /var/cache/nginx
USER nginxuser

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]