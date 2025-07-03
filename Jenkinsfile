pipeline {
    agent { label 'jenkins_slave' }

    environment {
        REGISTRY_URL = 'harbor.registry.local'
        IMAGE_NAME = 'todo-api'
        FRONTEND_IMAGE_NAME = 'todo-frontend'
        IMAGE_TAG = "v${BUILD_NUMBER}"
    }

    options {
        skipDefaultCheckout()
    }

    stages {

        stage('Checkout Code') {
            steps {
                git branch: 'master', url: 'https://github.com/lujamaharjan/Devops.git'
            }
        }

        stage('Build Docker Image') {
            steps {
                dir('app/backend') {
                    script {
                        sh """
                            docker build -t ${REGISTRY_URL}/sachin/${IMAGE_NAME}:${IMAGE_TAG} .
                        """
                    }
                }
            }
        }

        stage('Build Frontend') {
            steps {
                dir('app/frontend') {
                     sh """
                            docker build -t ${REGISTRY_URL}/sachin/${FRONTEND_IMAGE_NAME}:${IMAGE_TAG} .
                        """
                }
            }
        }

        stage('Login to Harbor') {
            steps {
                script {
                     withCredentials([usernamePassword(
                        credentialsId: 'HARBOR_CREDENTIALS',
                        usernameVariable: 'HARBOR_USER',
                        passwordVariable: 'HARBOR_PASS'
                    )]) {
                        sh "docker login ${REGISTRY_URL} -u $HARBOR_USER -p $HARBOR_PASS"
                        
                    }
                }
            }
        }

        stage('Push Docker Image') {
            steps {
                 script {
                    sh "docker push ${REGISTRY_URL}/sachin/${IMAGE_NAME}:${IMAGE_TAG}"
                    sh "docker push ${REGISTRY_URL}/sachin/${FRONTEND_IMAGE_NAME}:${IMAGE_TAG}"
                }
            }
        }

        stage('Cleanup') {
            steps {
                script {
                    sh "docker rmi ${REGISTRY_URL}/sachin/${IMAGE_NAME}:${IMAGE_TAG} || true"
                    sh "docker rmi ${REGISTRY_URL}/sachin/${FRONTEND_IMAGE_NAME}:${IMAGE_TAG} || true"
                }
            }
        }
    }

    post {
        success {
            emailext(
                to: 'sachin.maharjan@dishhome.com.np',
                subject: "Build SUCCESS: ${env.JOB_NAME} #${env.BUILD_NUMBER}",
                body: "The build completed successfully.\nSee details at ${env.BUILD_URL}console"
            )
        }
        failure {
            emailext(
                to: 'sachin.maharjan@dishhome.com.np',
                subject: "Build FAILED: ${env.JOB_NAME} #${env.BUILD_NUMBER}",
                body: "The build failed.\nCheck logs at ${env.BUILD_URL}console"
            )
        }
    }
}
