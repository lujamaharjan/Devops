FROM nginx:alpine

RUN rm -rf /usr/share/nginx/html/*
COPY . /usr/share/nginx/html

RUN addgroup -g 1001 nginxgroup 
RUN adduser nginxuser
RUN chown -R nginxuser:nginxuser /usr/share/nginx/html
USER nginxuser

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]