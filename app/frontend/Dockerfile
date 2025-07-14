FROM nginx:alpine

RUN rm -rf /usr/share/nginx/html/*
COPY . /usr/share/nginx/html

RUN addgroup -g 1001 nginxgroup 
RUN adduser -D -u 1001 -G nginxgroup -h /usr/share/nginx/html nginxuser
RUN chown -R nginxuser:nginxgroup /usr/share/nginx/html
USER nginxuser

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]