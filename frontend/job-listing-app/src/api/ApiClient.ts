import axios, { type AxiosInstance, type AxiosRequestConfig, type AxiosResponse, AxiosError } from 'axios';

class ApiClient {
    private readonly axios: AxiosInstance;

    constructor(baseURL: string) {
        this.axios = axios.create({
            baseURL,
            timeout: 5000,
        });

        this.initializeRequestInterceptor();
        this.initializeResponseInterceptor();
    }

    private initializeRequestInterceptor() {
        this.axios.interceptors.request.use(
            (config) => {
                const token = localStorage.getItem('token');
                if (token && config.headers) {
                    config.headers['Authorization'] = token;
                }
                return config;
            },
            (error: AxiosError) => Promise.reject(error)
        );
    }

    private initializeResponseInterceptor() {
        this.axios.interceptors.response.use(
            (response: AxiosResponse) => response,
            (error: AxiosError) => {
                return Promise.reject(error);
            }
        );
    }

    public get<T>(url: string, config?: AxiosRequestConfig): Promise<AxiosResponse<T>> {
        return this.axios.get<T>(url, config);
    }

    public post<T>(url: string, data?: unknown, config?: AxiosRequestConfig): Promise<AxiosResponse<T>> {
        return this.axios.post<T>(url, data, config);
    }

    public put<T>(url: string, data?: unknown, config?: AxiosRequestConfig): Promise<AxiosResponse<T>> {
        return this.axios.put<T>(url, data, config);
    }

    public delete<T>(url: string, config?: AxiosRequestConfig): Promise<AxiosResponse<T>> {
        return this.axios.delete<T>(url, config);
    }
}

export const apiClient = new ApiClient('http://localhost:8080/api');
