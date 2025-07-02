pipeline {
    agent { label 'jenkins_slave' }

    environment {
        REGISTRY_URL = 'https://harbor.registry.local'
        IMAGE_NAME = 'todo-api'
        IMAGE_TAG = "v${BUILD_NUMBER}"
        HARBOR_CREDENTIALS = credentials('harbor-credentials') // Jenkins credential ID
    }

    options {
        skipDefaultCheckout()
    }

    stages {

        stage('Checkout Code') {
            steps {
                git branch: 'master', url: 'https://github.com/your-username/your-repo.git'
            }
        }

        stage('Build Docker Image') {
            steps {
                dir('app/backend') {
                    script {
                        sh """
                            docker build -t ${REGISTRY_URL}/${IMAGE_NAME}:${IMAGE_TAG} .
                        """
                    }
                }
            }
        }

        stage('Login to Harbor') {
            steps {
                script {
                    sh """
                       docker login ${REGISTRY_URL} -u ${HARBOR_CREDENTIALS_USR} -p ${HARBOR_CREDENTIALS_PSW}
                    """
                }
            }
        }

        stage('Push Docker Image') {
            steps {
                script {
                    sh """
                        docker push ${REGISTRY_URL}/${IMAGE_NAME}:${IMAGE_TAG}
                    """
                }
            }
        }

        stage('Cleanup') {
            steps {
                script {
                    sh """
                        docker rmi ${REGISTRY_URL}/${IMAGE_NAME}:${IMAGE_TAG} || true
                    """
                }
            }
        }
    }

    post {
        failure {
            echo "Build failed!"
        }
        success {
            echo "Image pushed: ${REGISTRY_URL}/${IMAGE_NAME}:${IMAGE_TAG}"
        }
    }
}
