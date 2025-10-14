"use client"

import React from 'react';
import { RegisterRequest } from '../types/auth';

export default function Register() {

    const postData = async () => {
        const request: RegisterRequest = {
            name: 'Daniel Azevedo',
            userName: "daniazevedo97",
            email: "daniazevedo685@gmail.com",
            password: "Abc123abc123!"
        }

        const response = await fetch('https://localhost:7060/api/Auth/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(request),
        });

            const result = await response.json();
            console.log(result);
    };

    return (
        <div>
            <button onClick={postData} type="submit">Submit</button>
        </div>
    )
}